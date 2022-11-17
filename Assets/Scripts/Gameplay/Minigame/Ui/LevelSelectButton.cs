using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Image _checkbox;

    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetUp(bool interactable, bool done, UnityAction callback)
    {
        _button.onClick.RemoveAllListeners();
        _text.color = interactable ? Color.white : Color.gray;
        _button.interactable = interactable;
        _button.onClick.AddListener(callback);
        _checkbox.enabled = done;
    }
}