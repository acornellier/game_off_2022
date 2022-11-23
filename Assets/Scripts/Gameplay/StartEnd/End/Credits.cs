using TMPro;
using UnityEngine;
using Zenject;

public class Credits : MonoBehaviour
{
    [SerializeField] TMP_Text _polaroid1Text;
    [SerializeField] GameObject _bestBuddies;
    [SerializeField] GameObject _wedding;
    [SerializeField] GameObject _children;

    [Inject] PersistentDataManager _persistentDataManager;

    void Awake()
    {
        var ending = _persistentDataManager.data.gamesBeat >= 16
            ? 3
            : _persistentDataManager.data.gamesBeat >= 12
                ? 2
                : 1;

        _polaroid1Text.text = $"Ending {ending} unlocked!";

        _bestBuddies.gameObject.SetActive(ending == 1);
        _wedding.gameObject.SetActive(ending == 2);
        _children.gameObject.SetActive(ending == 3);
    }
}