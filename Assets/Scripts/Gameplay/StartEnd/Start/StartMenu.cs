using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    [SerializeField] Button _newGameButton;
    [SerializeField] TMP_Text _newGameText;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;

    void Start()
    {
        if (_persistentDataManager.data.cinematicsDone.IsEmpty())
        {
            _continueButton.gameObject.SetActive(false);
            _newGameText.text = "Start";
            EventSystem.current.SetSelectedGameObject(_newGameButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
        }
    }

    public void NewGame()
    {
        PersistentDataManager.Reset();
        _sceneLoader.SaveAndLoadScene("Intro");
    }

    public void Continue()
    {
        _sceneLoader.SaveAndLoadScene("Main");
    }
}