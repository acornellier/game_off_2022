using TMPro;
using UnityEngine;
using Zenject;

public class OverworldUi : MonoBehaviour
{
    [SerializeField] TMP_Text _level;

    [Inject] PersistentDataManager _persistentDataManager;

    void Start()
    {
        _level.text = $"Level: {_persistentDataManager.data.level}";
    }
}