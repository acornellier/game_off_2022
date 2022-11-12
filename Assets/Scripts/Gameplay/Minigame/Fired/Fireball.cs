using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _explosionRadius;

    readonly Collider2D[] _results = new Collider2D[32];

    void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * Vector3.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<Target>();
        if (!target) return;

        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _explosionRadius, _results);
        var firedCount = 0;

        for (var i = 0; i < size; ++i)
        {
            var overlapped = _results[i];
            var target2 = overlapped.GetComponent<Target>();
            if (!target2)
                continue;

            Utilities.DestroyGameObject(target2.gameObject);
            ++firedCount;
        }

        var firedGame = FindObjectOfType<FiredGame>();
        firedGame.OnFired(firedCount);
        Utilities.DestroyGameObject(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}