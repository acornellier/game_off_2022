using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class OverworldPlayer : Player
{
    [SerializeField] float _speed = 10;

    Rigidbody2D _body;
    InputActions.TopDownActions _inputActions;

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
        var moveInput = _inputActions.Move.ReadValue<Vector2>();
        var movement = _speed * Time.fixedDeltaTime * moveInput;
        _body.MovePosition((Vector2)transform.position + movement);
    }
}