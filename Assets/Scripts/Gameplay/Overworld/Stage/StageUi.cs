using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class StageUi : MonoBehaviour
{
    [SerializeField] GameObject _wrapper;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _levelsComplete;
    [SerializeField] Button _startButton;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;

    Stage _stage;

    void Awake()
    {
        _wrapper.SetActive(false);
        _startButton.onClick.AddListener(RunStage);
    }

    public void Activate(Stage stage)
    {
        _stage = stage;

        var stageData = _persistentDataManager.data.GetStageData(stage.id);

        stageData ??= new StageData();
        _wrapper.SetActive(true);

        _title.text = stage.title;
        _levelsComplete.text =
            $"Levels complete: {stageData.maxLevelIndexCompleted + 1}/{stage.levels.Count}";

        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    public void Deactivate()
    {
        _stage = null;
        _wrapper.SetActive(false);
    }

    public void RunStage()
    {
        _sceneLoader.SaveAndLoadScene(_stage.id);
    }
}