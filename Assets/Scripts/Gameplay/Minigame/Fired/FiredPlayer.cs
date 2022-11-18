using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FiredPlayer : Player
{
    [SerializeField] FiredPlayerArms _arms;
    [SerializeField] FiredPlayerLegs _legs;
    [SerializeField] AudioSource _shootSource;
    [SerializeField] AudioClip _shootClip;
    [SerializeField] float _speed = 10;
    [SerializeField] Fireball _fireballPrefab;
    [SerializeField] float _fireballCooldown;

    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

    float _timeUntilFireballAllowed;
    bool _fireballPending;

    void Awake()
    {
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
    }

    public override void DisableControls()
    {
        _inputActions.Disable();
    }

    void FixedUpdate()
    {
        UpdateMovement();
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
        _arms.Shoot();
        _shootSource.PlayOneShot(_shootClip);
        _timeUntilFireballAllowed = _fireballCooldown;
        _fireballPending = false;
    }

    void UpdateMovement()
    {
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        var movement = _speed * Time.fixedDeltaTime * moveInput;
        _body.MovePosition((Vector2)transform.position + new Vector2(movement.x, 0));
        _legs.SetMovement(movement.x);
    }
}