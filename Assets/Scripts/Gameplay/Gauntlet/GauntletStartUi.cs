using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class GauntletStartUi : MonoBehaviour
{
    [SerializeField] Image _wrapper;
    [SerializeField] Button _startButton;
    [SerializeField] Dialogue[] _dialogues;
    [SerializeField] GameObject _choiceUi;
    [SerializeField] Button _defaultButton;

    [Inject] DialogueManager _dialogueManager;
    [Inject] SceneLoader _sceneLoader;

    void Awake()
    {
        _wrapper.gameObject.SetActive(false);
        _choiceUi.gameObject.SetActive(false);
        _startButton.onClick.AddListener(RunStage);
    }

    public void Activate()
    {
        _wrapper.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    public void Deactivate()
    {
        _wrapper.gameObject.SetActive(false);
    }

    void RunStage()
    {
        StartCoroutine(CO_Run());
    }

    IEnumerator CO_Run()
    {
        _wrapper.gameObject.SetActive(false);
        _dialogueManager.StartDialogue(_dialogues);
        yield return new WaitWhile(() => _dialogueManager.isActive);
        _choiceUi.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_defaultButton.gameObject);
    }

    public void Yes()
    {
        _sceneLoader.SaveAndLoadScene("Gauntlet");
    }

    public void No()
    {
        _choiceUi.SetActive(false);
        _wrapper.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }
}