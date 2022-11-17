using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Explosion : MonoBehaviour
{
    [SerializeField] float _explosionTime = 0.3f;

    bool _exploded;
    float _explosionStartTime;
    readonly List<Target> _alreadyHit = new();

    void Awake()
    {
        Utilities.DestroyGameObject(gameObject, _explosionTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<Target>();
        if (!target || _alreadyHit.Contains(target)) return;

        _alreadyHit.Add(target);

        target.TakeDamage(1);
    }
}