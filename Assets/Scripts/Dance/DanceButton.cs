using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DanceButton : MonoBehaviour
{
    [SerializeField] KeyCode _key;

    SpriteRenderer _renderer;
    Color _originalColor;

    readonly Queue<DanceArrow> _activeArrows = new();

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
        
        if (!Input.GetKeyDown(_key) || _activeArrows.IsEmpty())
            return;

        Utilities.DestroyGameObject(_activeArrows.Dequeue().gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var danceArrow = col.GetComponent<DanceArrow>();
        if (danceArrow) 
            _activeArrows.Enqueue(danceArrow);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var danceArrow = col.GetComponent<DanceArrow>();
        if (danceArrow && !_activeArrows.IsEmpty())
            _activeArrows.Dequeue();
    }
}
