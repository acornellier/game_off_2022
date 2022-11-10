using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DanceButton : MonoBehaviour
{
    [SerializeField] KeyCode _key;

    SpriteRenderer _renderer;
    Color _originalColor;

    readonly List<DanceArrow> _activeArrows = new();

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
    }

    void Update()
    {
        _renderer.color = Input.GetKey(_key)
            ? Color.Lerp(_originalColor, Color.white, 0.5f)
            : _originalColor;

        if (!Input.GetKeyDown(_key))
            return;

        if (_activeArrows.IsEmpty())
            return;

        var firstArrow = _activeArrows.First();
        _activeArrows.RemoveAt(0);
        Utilities.DestroyGameObject(firstArrow.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var danceArrow = col.GetComponent<DanceArrow>();
        if (danceArrow)
            _activeArrows.Add(danceArrow);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var danceArrow = col.GetComponent<DanceArrow>();
        if (danceArrow && _activeArrows.Contains(danceArrow))
            _activeArrows.Remove(danceArrow);
    }
}