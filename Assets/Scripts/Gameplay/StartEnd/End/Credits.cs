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
    [Inject] SceneLoader _sceneLoader;

    int _ending;

    void Awake()
    {
        _ending = _persistentDataManager.data.gamesBeat >= 16
            ? 3
            : _persistentDataManager.data.gamesBeat >= 12
                ? 2
                : 1;

        _polaroid1Text.text = $"Ending {_ending} unlocked!";

        _bestBuddies.gameObject.SetActive(_ending == 1);
        _wedding.gameObject.SetActive(_ending >= 2);
        _children.gameObject.SetActive(_ending >= 3);
    }

    public void EndingDone()
    {
        if (_ending <= 2)
            _sceneLoader.SaveAndLoadScene("Start");
    }

    public void SecretEndingDone()
    {
        _sceneLoader.SaveAndLoadScene("Start");
    }
}