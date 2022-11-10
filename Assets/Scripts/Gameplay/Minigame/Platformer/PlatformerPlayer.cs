using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimancerComponent))]
public class PlatformerPlayer : Player
{
    InputActions.PlatformerActions _inputActions;

    // [SerializeField] PlayerAudio playerAudio;
    [SerializeField] Collider2D _cleets;

    [SerializeField] Stats _stats;
    // [SerializeField] Animations animations;

    [NonSerialized] public bool isDead;

    Collider2D _collider;
    Rigidbody2D _body;
    AnimancerComponent _animancer;
    ContactFilter2D _groundMask;

    float _jumpInputTimestamp = float.NegativeInfinity;
    bool _isGrounded;
    bool _isJumping;
    float _jumpingTimestamp = float.NegativeInfinity;
    bool _isFalling;
    float _landTimestamp = float.NegativeInfinity;

    readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[8];

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _groundMask.layerMask = LayerMask.GetMask("Collision");
        _inputActions = new InputActions().Platformer;
        _inputActions.Jump.performed += _ => _jumpInputTimestamp = Time.time;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        UpdateGrounded();
        UpdateRunning();
        UpdateJumping();
        UpdateDirection();
        // UpdateAnimations();
    }

    public override void EnableControls()
    {
        _inputActions.Enable();
    }

    public override void DisableControls()
    {
        _inputActions.Disable();
    }

    void OnFootstep()
    {
        // playerAudio.Footstep();
    }

    void UpdateGrounded()
    {
        _isGrounded = IsGrounded();
    }

    bool IsGrounded()
    {
        var numHits = _collider.Cast(Vector2.down, _groundMask, _hitBuffer, 0.15f);
        for (var hitIndex = 0; hitIndex < numHits; hitIndex++)
        {
            var hit = _hitBuffer[hitIndex];
            if (Vector2.Angle(hit.normal, Vector2.up) < 60)
                return true;
        }

        return false;
    }

    void UpdateRunning()
    {
        var moveInput = _inputActions.Move.ReadValue<float>();
        var velocity = _body.velocity;
        var velocityDirection = Math.Sign(velocity.x);

        if (moveInput != 0)
        {
            var acceleration = velocityDirection == Math.Sign(moveInput)
                ? _stats.acceleration
                : _stats.turnAcceleration;

            velocity.x += moveInput * acceleration * Time.fixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -_stats.maxSpeed, _stats.maxSpeed);
        }
        else if (velocityDirection != 0)
        {
            if (Mathf.Abs(velocity.x) < 0.01f)
            {
                velocity.x = 0;
            }
            else
            {
                velocity.x -= velocityDirection * _stats.deceleration * Time.fixedDeltaTime;

                if (Math.Sign(velocity.x) != velocityDirection)
                    velocity.x = 0;
            }
        }

        _body.velocity = velocity;
    }

    void UpdateJumping()
    {
        _body.gravityScale = _stats.gravityForce;

        if ((_isJumping && _body.velocity.y < 0) || _body.velocity.y < -5f)
        {
            _isFalling = true;
            _body.gravityScale *= _stats.fallingGravityMultiplier;
        }

        if (_isGrounded && (_isFalling || (_isJumping && Time.time - _jumpingTimestamp > 0.1f)))
        {
            _isFalling = false;
            _isJumping = false;
            _landTimestamp = Time.time;
            // playerAudio.Land();
        }

        if (_isGrounded && Time.time - _jumpInputTimestamp < _stats.jumpInputBuffer)
            Jump();
    }

    public void Jump(float multiplier = 1f)
    {
        _isJumping = true;
        _jumpingTimestamp = Time.time;
        _jumpInputTimestamp = 0;
        _body.velocity = new Vector2(_body.velocity.x, 0);
        _body.AddForce(new Vector2(0, _stats.jumpForce * multiplier), ForceMode2D.Impulse);
        // playerAudio.Jump();
    }

    void UpdateDirection()
    {
        var moveInput = _inputActions.Move.ReadValue<float>();

        if ((moveInput < 0 && transform.localScale.x > 0) ||
            (moveInput > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // void UpdateAnimations()
    // {
    //     if (isDead)
    //         return;
    //
    //     if (_isFalling)
    //         _animancer.Play(animations.fall);
    //     else if (_isJumping)
    //         _animancer.Play(animations.jump);
    //     else if (Time.time - _landTimestamp < 0.2f)
    //         _animancer.Play(animations.land);
    //     else if (Mathf.Abs(_body.velocity.x) > 0.01)
    //         _animancer.Play(animations.walk);
    //     else
    //         _animancer.Play(animations.idle);
    // }

    public void Die()
    {
        isDead = true;

        if (_cleets) _cleets.enabled = false;
        DisableControls();

        // playerAudio.Die();

        // var state = _animancer.Play(animations.die);
        // yield return state;

        // Respawn();
    }

    // public void Respawn()
    // {
    //     transform.position = _checkpointManager.CurrentCheckpoint.position;
    // }

    [Serializable]
    class Stats
    {
        public float maxSpeed = 5;
        public float turnAcceleration = 5;
        public float acceleration = 5;
        public float deceleration = 5;

        public float jumpForce = 8;
        public float gravityForce = 1;
        public float fallingGravityMultiplier = 1.2f;
        public float jumpInputBuffer = 0.1f;
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip jump;
        public AnimationClip fall;
        public AnimationClip land;
        public AnimationClip die;
    }
}