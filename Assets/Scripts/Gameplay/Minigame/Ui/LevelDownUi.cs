using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelDownUi : MonoBehaviour
{
    [SerializeField] ShrinkingMalafor _shrinkingMalafor;
    [SerializeField] GameObject _description;

    void OnEnable()
    {
        _description.SetActive(false);
        _shrinkingMalafor.ResetSize();
    }

    public IEnumerator CO_Run()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_shrinkingMalafor.CO_Shrink());
        _description.SetActive(true);
        yield return new WaitUntil(
            () => Keyboard.current.spaceKey.isPressed ||
                  Keyboard.current.enterKey.isPressed
        );
    }
}