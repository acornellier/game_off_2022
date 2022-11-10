using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatAction : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] TMP_Text _text;

    public void OnSelect(BaseEventData eventData)
    {
        _text.color = Color.red;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _text.color = Color.white;
    }
}