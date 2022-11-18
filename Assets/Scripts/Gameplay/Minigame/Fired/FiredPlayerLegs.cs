using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class FiredPlayerLegs : MonoBehaviour
{
    [SerializeField] AnimationClip _idle;
    [SerializeField] AnimationClip _strafeLeft;
    [SerializeField] AnimationClip _strafeRight;

    AnimancerComponent _animancer;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    public void SetMovement(float movement)
    {
        _animancer.Play(
            movement > 0
                ? _strafeRight
                : movement < 0
                    ? _strafeLeft
                    : _idle
        );
    }
}