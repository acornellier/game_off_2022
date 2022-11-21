using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    [SerializeField] GameObject _names;
    [SerializeField] GameObject _playtesters;
    [SerializeField] Button _continueButton2;

    void OnEnable()
    {
        _names.SetActive(true);
        _playtesters.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
    }

    public void Continue()
    {
        _names.SetActive(false);
        _playtesters.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_continueButton2.gameObject);
    }
}