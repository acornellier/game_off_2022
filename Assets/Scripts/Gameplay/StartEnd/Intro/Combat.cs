using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [SerializeField] GameObject _optionsWrapper;
    [SerializeField] Button[] _options;
    [SerializeField] UnityEvent _onSelectEvent;
    [SerializeField] IntroFireball _fireball;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(_options[0].gameObject);
    }

    public void OnClick(int colorIndex)
    {
        _fireball.SetColorIndex(colorIndex);
        _optionsWrapper.SetActive(false);
        _onSelectEvent.Invoke();
    }
}