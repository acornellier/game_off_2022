using TMPro;
using UnityEngine;
using Zenject;

public class StageUi : MonoBehaviour
{
    [SerializeField] GameObject _wrapper;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _requiredLevel;
    [SerializeField] TMP_Text _levelsLost;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;

    Stage _stage;
    NodeEvent _nodeEvent;

    void Awake()
    {
        _wrapper.SetActive(false);
    }

    public void Activate(Stage stage, NodeEvent nodeEvent)
    {
        _stage = stage;
        _nodeEvent = nodeEvent;

        var stageData = _persistentDataManager.data.GetStageData(stage.name);

        stageData ??= new StageData();

        _wrapper.SetActive(true);

        _title.text = $"Stage {stage.name}";
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
    }

    public void Deactivate()
    {
        _stage = null;
        _nodeEvent = null;
        _wrapper.SetActive(false);
    }

    public void RunStage()
    {
        if (_stage != null)
            _sceneLoader.LoadScene("Stage" + _stage.name);
    }
}