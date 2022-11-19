using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class FiredPlayerLegs : MonoBehaviour
{
    [SerializeField] AnimationClip _idleLeft;
    [SerializeField] AnimationClip _idleRight;
    [SerializeField] AnimationClip _strafeLeft;
    [SerializeField] AnimationClip _strafeRight;

    AnimancerComponent _animancer;
    float _lastMovement;

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
                    : _lastMovement > 0
                        ? _idleRight
                        : _idleLeft
        );

        if (movement != 0)
            _lastMovement = movement;
    }
}