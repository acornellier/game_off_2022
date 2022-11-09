using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayer : Player
{
    [SerializeField] float _speed = 10;
    [SerializeField] Animations _animations;

    AnimancerComponent _animancer;
    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

    Vector2Int _facingDirection = Vector2Int.down;

    public Action onInteract;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
        _inputActions = new InputActions().TopDown;
        _inputActions.Interact.performed += _ => onInteract?.Invoke();
    }

    void OnEnable()
    {
        EnableControls();
    }

    void OnDisable()
    {
        DisableControls();
    }

    public override void EnableControls()
    {
        _inputActions.Enable();
    }

    public override void DisableControls()
    {
        _inputActions.Disable();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
        UpdateAnimations();
    }

    public void Fireball()
    {
        // TODO: animate
    }

    void UpdateMovement()
    {
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        var movement = _speed * Time.fixedDeltaTime * moveInput;
        _body.MovePosition((Vector2)transform.position + movement);
    }

    void UpdateDirection()
    {
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        if (moveInput == default || moveInput == _facingDirection)
            return;

        SetFacingDirection(
            moveInput.y == 0
                ? Vector2Int.RoundToInt(moveInput)
                : new Vector2Int(0, Mathf.RoundToInt(moveInput.y))
        );
    }

    void SetFacingDirection(Vector2Int facingDirection)
    {
        _facingDirection = facingDirection;
    }

    void UpdateAnimations()
    {
        var directionalAnimationSet = GetDirectionalAnimationSet();
        _animancer.Play(directionalAnimationSet.GetClip(_facingDirection));
    }

    DirectionalAnimationSet GetDirectionalAnimationSet()
    {
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        return moveInput == default ? _animations.idle : _animations.walk;
    }

    [Serializable]
    class Animations
    {
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
    }
}