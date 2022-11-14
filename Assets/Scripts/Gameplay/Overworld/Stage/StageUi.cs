using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class StageUi : MonoBehaviour
{
    [SerializeField] Image _wrapper;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _levelsComplete;
    [SerializeField] Button _startButton;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] SceneLoader _sceneLoader;

    Stage _stage;

    void Awake()
    {
        _wrapper.gameObject.SetActive(false);
        _startButton.onClick.AddListener(RunStage);
    }

    public void Activate(Stage stage, bool isLeft)
    {
        _stage = stage;

        _wrapper.gameObject.SetActive(true);
        _wrapper.rectTransform.anchorMin = new Vector2(isLeft ? 0 : 1, 0.5f);
        _wrapper.rectTransform.anchorMax = new Vector2(isLeft ? 0 : 1, 0.5f);
        _wrapper.rectTransform.pivot = new Vector2(isLeft ? 0 : 1, 0.5f);

        var stageData = _persistentDataManager.data.GetStageData(stage.id);

        _title.text = stage.title;
        _levelsComplete.text =
            $"Levels complete: {stageData.maxLevelIndexCompleted + 1}/{stage.levels.Count}";

        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    public void Deactivate()
    {
        _stage = null;
        _wrapper.gameObject.SetActive(false);
    }

    void RunStage()
    {
        _sceneLoader.SaveAndLoadScene(_stage.id);
    }
}