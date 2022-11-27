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
    Collider2D _collider;
    InputActions.TopDownActions _inputActions;

    bool _animationsDisabled;
    Transform _throneExitSpot;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _inputActions = new InputActions().TopDown;
        _inputActions.Interact.performed += _ => OnInteract();
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

    void OnInteract()
    {
        if (_animancer.IsPlaying(_animations.sitThrone))
            StopSitThrone();
        else
            onInteract?.Invoke();
    }

    public void SitThrone(Transform sitSpot, Transform exitSpot)
    {
        print("SitThrone");
        _throneExitSpot = exitSpot;
        _collider.enabled = false;
        transform.position = sitSpot.position;
        _animancer.Play(_animations.sitThrone);
        _animationsDisabled = true;
        _inputActions.Move.Disable();
    }

    void StopSitThrone()
    {
        print("StopSitThrone");
        transform.position = _throneExitSpot.position;
        _animationsDisabled = false;
        UpdateAnimations();
        _inputActions.Move.Enable();
        _collider.enabled = true;
    }

    [Serializable]
    class Animations
    {
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
        public AnimationClip sitThrone;
    }
}