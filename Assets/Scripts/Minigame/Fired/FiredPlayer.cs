using UnityEngine;

public class FiredPlayer : MonoBehaviour
{
    [SerializeField] TopDownPlayer _player;
    [SerializeField] Fireball _fireballPrefab;
    [SerializeField] float _fireballCooldown;

    float _timeUntilFireballAllowed;

    void OnEnable()
    {
        _player.onInteract += OnFireball;
    }

    void OnDisable()
    {
        _player.onInteract -= OnFireball;
    }

    void Update()
    {
        _timeUntilFireballAllowed -= Time.deltaTime;
    }

    void OnFireball()
    {
        if (_timeUntilFireballAllowed > 0)
            return;

        Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
        _timeUntilFireballAllowed = _fireballCooldown;
    }
}