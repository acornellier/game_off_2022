using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FiredPlayer : Player
{
    [SerializeField] float _speed = 10;
    [SerializeField] Fireball _fireballPrefab;

    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _inputActions = new InputActions().TopDown;
        _inputActions.Interact.performed += OnInteract;
    }

    void OnEnable()
    {
        EnableControls();
    }

    void OnDisable()
    {
        DisableControls();
    }

    void FixedUpdate()
    {
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        var movement = _speed * Time.fixedDeltaTime * moveInput;
        _body.MovePosition((Vector2)transform.position + movement);
    }

    public override void EnableControls()
    {
        _inputActions.Enable();
    }

    public override void DisableControls()
    {
        _inputActions.Disable();
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
    }
}