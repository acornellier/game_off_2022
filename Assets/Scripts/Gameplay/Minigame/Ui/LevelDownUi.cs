using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelDownUi : MonoBehaviour
{
    [SerializeField] AudioSource _levelDownSource;
    [SerializeField] AudioClip _levelDownClip;
    [SerializeField] GameObject _description;

    void OnEnable()
    {
        _description.SetActive(false);
    }

    public IEnumerator CO_Run()
    {
        yield return new WaitForSeconds(0.5f);
        _levelDownSource.PlayOneShot(_levelDownClip);
        yield return new WaitForSeconds(1.5f);
        _description.SetActive(true);
        yield return new WaitUntil(
            () => Keyboard.current.spaceKey.isPressed ||
                  Keyboard.current.enterKey.isPressed
        );
    }
}