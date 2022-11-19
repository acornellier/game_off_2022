using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] Button _continueButton;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
    }
}