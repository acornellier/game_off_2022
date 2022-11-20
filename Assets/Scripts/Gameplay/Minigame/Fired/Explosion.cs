using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Explosion : MonoBehaviour
{
    [SerializeField] float _explosionTime = 0.3f;

    CircleCollider2D _collider;
    SpriteRenderer _renderer;

    bool _exploded;
    float _awakeTime;
    readonly List<Target> _alreadyHit = new();

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _awakeTime = Time.time;
        Utilities.DestroyGameObject(gameObject, _explosionTime * 4);
    }

    void Update()
    {
        if (Time.time - _awakeTime > _explosionTime)
        {
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<Target>();
        if (!target || _alreadyHit.Contains(target)) return;

        _alreadyHit.Add(target);

        target.TakeDamage(1);
    }
}