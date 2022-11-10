using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiCursor : MonoBehaviour
{
    [SerializeField] Image _image;

    void Update()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (!selected) return;

        _image.enabled = selected.activeInHierarchy;
        _image.transform.SetParent(selected.transform, false);
    }
}