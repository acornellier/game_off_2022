using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class Door : MonoBehaviour
{
    [SerializeField] AnimationClip _open;

    AnimancerComponent _animancer;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    public void Open()
    {
        var state = _animancer.Play(_open);
        state.NormalizedTime = 0;
        state.Speed = 1;
    }

    public void Close()
    {
        var state = _animancer.Play(_open);
        state.NormalizedTime = 1;
        state.Speed = -1;
    }
}