using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 8;

    Rigidbody2D _body;
    InputActions.PlayerActions _inputActions;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _inputActions = new InputActions().Player;
        _inputActions.Jump.performed += Jump;
    }

    void OnEnable()
    {
        _inputActions.Enable();
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

    void FixedUpdate()
    {
        var velocity = _body.velocity;
        var moveInput = _inputActions.Move.ReadValue<float>();
        velocity.x = _speed * moveInput;
        _body.velocity = velocity;
    }

    void Jump(InputAction.CallbackContext obj)
    {
        _body.velocity = new Vector2(_body.velocity.x, 0);
        _body.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
    }
}