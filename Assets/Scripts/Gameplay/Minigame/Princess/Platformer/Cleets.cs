using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Cleets : MonoBehaviour
{
    [SerializeField] PlatformerPlayer player;
    [SerializeField] float jumpMultiplier = 0.8f;

    Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var enemy = col.gameObject.GetComponent<Enemy>();
        if (!enemy)
            return;

        if (_collider.bounds.min.y - col.bounds.max.y < -0.5f)
            return;

        enemy.Die();
        player.Jump(jumpMultiplier);
    }
}