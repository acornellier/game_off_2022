using Animancer;
using UnityEngine;

public class IntroFireball : MonoBehaviour
{
    [SerializeField] AnimationClip[] _clips;

    AnimancerComponent _animancer;

    int _colorIndex;

    void OnEnable()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(_clips[_colorIndex]);
    }

    public void SetColorIndex(int colorIndex)
    {
        _colorIndex = colorIndex;
    }
}