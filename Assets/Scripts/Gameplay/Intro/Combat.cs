using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [SerializeField] GameObject _optionsWrapper;
    [SerializeField] Button[] _options;
    [SerializeField] UnityEvent _onSelectEvent;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(_options[0].gameObject);
    }

    public void OnClick()
    {
        _optionsWrapper.SetActive(false);
        _onSelectEvent.Invoke();
    }
}