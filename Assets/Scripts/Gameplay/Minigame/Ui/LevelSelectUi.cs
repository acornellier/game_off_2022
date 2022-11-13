using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class LevelSelectUi : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] LevelSelectButton[] _buttons;

    [Inject] MinigameManager _minigameManager;
    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;
    [Inject] Stage _stage;

    int _prevSelectedIndex = -1;

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

        _title.text = _stage.title;

        for (var i = 0; i < _buttons.Length; ++i)
        {
            var button = _buttons[i];

            if (i >= _stage.levels.Count)
            {
                button.gameObject.SetActive(false);
                continue;
            }

            button.gameObject.SetActive(true);

            var interactable = i == 0 || stageData.maxLevelIndexCompleted >= i - 1;
            var done = stageData.maxLevelIndexCompleted >= i;
            var levelIndex = i;
            button.SetUp($"Level {i + 1}", interactable, done, () => StartLevel(levelIndex));
        }

        var selectedIndex = _prevSelectedIndex == -1 ? 0 : _prevSelectedIndex;
        EventSystem.current.SetSelectedGameObject(_buttons[selectedIndex].gameObject);
    }

    public void ReturnToMain()
    {
        _sceneLoader.SaveAndLoadScene("Main");
    }

    void StartLevel(int levelIndex)
    {
        _prevSelectedIndex = levelIndex;
        _minigameManager.StartGame(levelIndex);
    }
}