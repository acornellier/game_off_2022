using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class FiredPlayerArms : MonoBehaviour
{
    [SerializeField] AnimationClip _idle;
    [SerializeField] AnimationClip _shootLeft;
    [SerializeField] AnimationClip _shootRight;

    AnimancerComponent _animancer;
    bool _lastShotLeft;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(_idle);
    }

    public void Shoot()
    {
        var state = _animancer.Play(_lastShotLeft ? _shootRight : _shootLeft);
        state.Events.OnEnd += () => _animancer.Play(_idle);
        _lastShotLeft = !_lastShotLeft;
    }
}