using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Hook : MonoBehaviour
{
    [SerializeField] float _hookSpeed = 0.1f;
    [SerializeField] float _hookGravity = .05f;
    [SerializeField] Sprite _goodHookSprite;
    [SerializeField] Sprite _badHookSprite;

    public Bounds bounds => _collider.bounds;

    Collider2D _collider;
    SpriteRenderer _spriteRenderer;

    float _hookPosition;
    float _hookPullVelocity;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public float GetNewPosition()
    {
        if (Mouse.current.leftButton.isPressed || Keyboard.current.spaceKey.isPressed)
            _hookPullVelocity += _hookSpeed * Time.fixedDeltaTime;

        _hookPullVelocity -= _hookGravity * Time.fixedDeltaTime;
        _hookPosition += _hookPullVelocity;
        _hookPosition = Mathf.Clamp01(_hookPosition);

        if (_hookPosition is <= 0 or >= 1)
            _hookPullVelocity = 0;

        return _hookPosition;
    }

    public void SetActive(bool active)
    {
        _spriteRenderer.GetComponent<SpriteRenderer>().sprite =
            active ? _goodHookSprite : _badHookSprite;
    }
}