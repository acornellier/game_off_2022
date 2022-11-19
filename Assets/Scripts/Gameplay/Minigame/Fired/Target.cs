using System;
using Animancer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Target : MonoBehaviour
{
    [SerializeField] AudioSource _dieSource;
    [SerializeField] AudioClip _dieClip;
    [SerializeField] TMP_Text _pointsText;
    [SerializeField] Image _healthBarFill;

    [SerializeField] AnimationClip _walkClip;
    [SerializeField] AnimationClip _banishClip;
    [SerializeField] AnimationClip _damagedClip;

    [SerializeField] int maxHealth = 1;
    [SerializeField] int points = 1;
    [SerializeField] float _speed;

    AnimancerComponent _animancer;
    Collider2D _collider;

    Action<int> _onFired;
    Vector3 _direction;
    float _health;
    bool _dead;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _collider = GetComponent<Collider2D>();
        _health = maxHealth;
        UpdateHealthBar();
        _animancer.Play(_walkClip);
    }

    void FixedUpdate()
    {
        if (_dead) return;

        transform.position += _speed * Time.fixedDeltaTime * _direction;
    }

    public void OnCreated(Action<int> onFired, Vector3 direction)
    {
        _onFired = onFired;
        _direction = direction;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (_pointsText)
                _pointsText.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_dead) return;

        _health -= damage;
        UpdateHealthBar();

        if (_health <= 0)
            Die();
        else if (_damagedClip != null)
            _animancer.Play(_damagedClip);
    }

    void Die()
    {
        _dead = true;
        _collider.enabled = false;

        _onFired?.Invoke(points);
        _pointsText.gameObject.SetActive(true);
        _pointsText.text = $"+{points}";

        _dieSource.PlayOneShot(_dieClip);
        var state = _animancer.Play(_banishClip);
        state.Events.OnEnd += () => Utilities.DestroyGameObject(gameObject);
    }

    void UpdateHealthBar()
    {
        if (_healthBarFill != null)
            _healthBarFill.fillAmount = _health / maxHealth;
    }
}