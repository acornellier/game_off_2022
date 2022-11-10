using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Target : MonoBehaviour
{
    Vector3 _direction;
    float _speed;

    void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * _direction;
    }

    public void OnCreated(Vector3 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }
}