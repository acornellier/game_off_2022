using UnityEngine;
using Zenject;

public class GauntletUnlocker : MonoBehaviour
{
    [SerializeField] GameObject _gauntlet;

    [Inject] PersistentDataManager _persistentDataManager;

    void Awake()
    {
        _gauntlet.SetActive(_persistentDataManager.data.gauntletUnlocked);
    }
}