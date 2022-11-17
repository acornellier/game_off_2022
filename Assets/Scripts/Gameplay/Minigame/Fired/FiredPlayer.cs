using UnityEngine;
using System;
using Animancer;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FiredPlayer : Player
{
    [SerializeField] float _speed = 10;
    [SerializeField] Animations _animations;
    [SerializeField] Fireball _fireballPrefab;
    [SerializeField] float _fireballCooldown;

    AnimancerComponent _animancer;
    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

    Vector2Int _facingDirection = Vector2Int.down;
    bool _animationsDisabled;
    float _timeUntilFireballAllowed;
    bool _fireballPending;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
        _inputActions = new InputActions().TopDown;
    }

    void OnEnable()
    {
        EnableControls();
    }

    void OnDisable()
    {
        DisableControls();
    }

    void Update()
    {
        _timeUntilFireballAllowed -= Time.deltaTime;

        if (_inputActions.Interact.IsPressed() || _fireballPending)
            OnFireball();
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
    }

    void OnFireball()
    {
        if (_timeUntilFireballAllowed > 0)
        {
            if (_timeUntilFireballAllowed < 0.3f)
                _fireballPending = true;
            return;
        }

        Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
        _timeUntilFireballAllowed = _fireballCooldown;
        _fireballPending = false;
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
        if (_animationsDisabled) return;

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