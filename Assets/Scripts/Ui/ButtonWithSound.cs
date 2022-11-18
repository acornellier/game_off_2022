using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonWithSound : MonoBehaviour
{
    [SerializeField] AudioClip _clickClip;
    [SerializeField] AudioClip _hoverClip;

    [Inject] GlobalSounds _globalSounds;

    public void OnHover()
    {
        _globalSounds.PlayOneShotSound(_hoverClip);
    }

    public void OnSubmit()
    {
        _globalSounds.PlayOneShotSound(_clickClip);
    }
}