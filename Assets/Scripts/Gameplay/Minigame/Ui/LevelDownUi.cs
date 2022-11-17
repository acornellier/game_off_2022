using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelDownUi : MonoBehaviour
{
    [SerializeField] GameObject _description;

    void OnEnable()
    {
        _description.SetActive(false);
    }

    public IEnumerator CO_Run()
    {
        yield return new WaitForSeconds(2f);
        _description.SetActive(true);
        yield return new WaitUntil(
            () => Keyboard.current.spaceKey.isPressed ||
                  Keyboard.current.enterKey.isPressed
        );
    }
}