using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayer : Player
{
    [SerializeField] float _speed = 10;
    [SerializeField] PlayerAudio _audio;
    [SerializeField] Animations _animations;

    public Action onInteract;
    public Vector2Int facingDirection = Vector2Int.down;

    AnimancerComponent _animancer;
    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

    bool _animationsDisabled;

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
        _animationsDisabled = false;
    }

    public override void DisableControls()
    {
        _inputActions.Disable();
        _animationsDisabled = true;
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
        UpdateAnimations();
    }

    public void Footstep()
    {
        _audio.Footstep();
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
        if (moveInput == default || moveInput == facingDirection)
            return;

        SetFacingDirection(
            moveInput.y == 0
                ? Vector2Int.RoundToInt(moveInput)
                : new Vector2Int(0, Mathf.RoundToInt(moveInput.y))
        );
    }

    void SetFacingDirection(Vector2Int newDirection)
    {
        facingDirection = newDirection;
    }

    void UpdateAnimations()
    {
        if (_animationsDisabled) return;

        var directionalAnimationSet = GetDirectionalAnimationSet();
        _animancer.Play(directionalAnimationSet.GetClip(facingDirection));
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