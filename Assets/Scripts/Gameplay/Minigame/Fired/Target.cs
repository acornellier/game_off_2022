using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Target : MonoBehaviour
{
    [SerializeField] AudioSource _dieSource;
    [SerializeField] AudioClip _dieClip;
    [SerializeField] TMP_Text _pointsText;
    [SerializeField] Image _healthBarFill;

    [SerializeField] int maxHealth = 1;
    [SerializeField] int points = 1;
    [SerializeField] float _speed;

    SpriteRenderer _spriteRenderer;

    Action<int> _onFired;
    Vector3 _direction;
    float _health;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = maxHealth;
        UpdateHealthBar();
    }

    void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * _direction;
    }

    public void OnCreated(Action<int> onFired, Vector3 direction)
    {
        _onFired = onFired;
        _direction = direction;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        UpdateHealthBar();

        if (_health <= 0)
            Die();
    }

    void Die()
    {
        _onFired?.Invoke(points);
        _spriteRenderer.enabled = false;
        _pointsText.gameObject.SetActive(true);
        _pointsText.text = $"+{points}";
        _dieSource.PlayOneShot(_dieClip);
        Utilities.DestroyGameObject(gameObject, 0.2f);
    }

    void UpdateHealthBar()
    {
        if (_healthBarFill != null)
            _healthBarFill.fillAmount = _health / maxHealth;
    }
}