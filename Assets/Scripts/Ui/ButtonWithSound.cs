using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonWithSound : MonoBehaviour
{
    [SerializeField] AudioClip _clickClip;

    [Inject] GlobalSounds _globalSounds;

    public void OnSubmit()
    {
        _globalSounds.PlayOneShotSound(_clickClip);
    }
}