using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class StageUi : MonoBehaviour
{
    [SerializeField] GameObject _wrapper;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _requiredLevel;
    [SerializeField] TMP_Text _levelsLost;
    [SerializeField] Button _startButton;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;

    Stage _stage;

    void Awake()
    {
        _wrapper.SetActive(false);
    }

    public void Activate(Stage stage)
    {
        _stage = stage;

        var stageData = _persistentDataManager.data.GetStageData(stage.id);

        stageData ??= new StageData();

        _wrapper.SetActive(true);

        _title.text = stage.title;
        _levelsLost.text = $"Levels Lost: {stageData.levelsLost}/{stage.maxLevelsToLose}";

        if (stage.maxLevelRequired > 0 &&
            _persistentDataManager.data.level > stage.maxLevelRequired)
        {
            _requiredLevel.gameObject.SetActive(true);
            _requiredLevel.text = $"Power Maximum: {stage.maxLevelRequired}";
        }
        else
        {
            _requiredLevel.gameObject.SetActive(false);
        }

        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    public void Deactivate()
    {
        _stage = null;
        _wrapper.SetActive(false);
    }

    public void RunStage()
    {
        if (_stage != null)
            _sceneLoader.SaveAndLoadScene("Stage" + _stage.id);
    }
}