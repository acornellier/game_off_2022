using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class LevelSelectUi : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] Button[] _buttons;

    [Inject] MinigameManager _minigameManager;
    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;
    [Inject] Stage _stage;

    void OnEnable()
    {
        Initialize();
    }

    void Initialize()
    {
        var stageData = _persistentDataManager.data.GetStageData(_stage.id);

        stageData ??= new StageData();

        if (_stage.levels.Count > _buttons.Length)
            throw new Exception("Max number of levels exceeded");

        for (var i = 0; i < _buttons.Length; ++i)
        {
            var button = _buttons[i];

            if (i >= _stage.levels.Count)
            {
                button.gameObject.SetActive(false);
                continue;
            }

            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();

            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = $"Level {i + 1}";

            var interactable = i == 0 || stageData.maxLevelIndexCompleted >= i - 1;
            text.color = interactable ? Color.white : Color.gray;
            button.interactable = interactable;
            var levelIndex = i;
            button.onClick.AddListener(() => StartLevel(levelIndex));
        }

        EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
        _title.text = _stage.title;
    }

    public void ReturnToMain()
    {
        _sceneLoader.SaveAndLoadScene("Main");
    }

    void StartLevel(int levelIndex)
    {
        _minigameManager.StartGame(levelIndex);
    }
}