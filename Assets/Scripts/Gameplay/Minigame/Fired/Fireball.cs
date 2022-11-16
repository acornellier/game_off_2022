using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] Explosion _explosion;

    float _explosionStartTime;

    void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * Vector3.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<Target>();
        if (!target) return;

        Instantiate(_explosion, transform.position, Quaternion.identity);
        Utilities.DestroyGameObject(gameObject);
    }
}