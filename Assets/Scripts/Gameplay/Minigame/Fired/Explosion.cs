using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Explosion : MonoBehaviour
{
    [SerializeField] float _explosionTime = 0.3f;

    bool _exploded;
    float _explosionStartTime;

    void Awake()
    {
        Utilities.DestroyGameObject(gameObject, _explosionTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<Target>();
        if (!target) return;

        Utilities.DestroyGameObject(target.gameObject);
        var firedGame = FindObjectOfType<FiredGame>();
        firedGame.OnFired(1);
    }
}