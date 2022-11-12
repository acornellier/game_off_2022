using System.Collections;
using UnityEngine;
using Zenject;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] MinigameUi _minigameUi;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] Stage _stage;

    float _timeRemaining;
    float _lastTimeRemaining;
    int _totalLevelsLost;

    public void StartGame(int levelIndex)
    {
        StartCoroutine(CO_StartGame(levelIndex));
    }

    IEnumerator CO_StartGame(int levelIndex)
    {
        var minigame = Instantiate(_stage.levels[levelIndex]);

        _timeRemaining = minigame.maxTime;

        _minigameUi.ShowInGameUi(minigame.maxTime);
        minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                if (minigame.isDone)
                    return true;

                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                    return true;

                _minigameUi.SetTimeRemaining(_timeRemaining, minigame.maxTime);
                return false;
            }
        );

        minigame.End();

        var fracTime = _timeRemaining / minigame.maxTime;
        var levelsLost = fracTime switch
        {
            >= 0.5f => 4,
            >= 0.33f => 3,
            >= 0.166f => 2,
            > 0 => 1,
            _ => 0,
        };

        var result = new MinigameResult
        {
            maxTime = minigame.maxTime,
            timeRemaining = _timeRemaining,
            levelsLost = levelsLost,
        };

        var stageData = _persistentDataManager.data.GetStageData(_stage.id);
        if (result.success && levelIndex > stageData.maxLevelIndexCompleted)
            stageData.maxLevelIndexCompleted = levelIndex;

        Utilities.DestroyGameObject(minigame.gameObject);

        yield return StartCoroutine(_minigameUi.ShowSummary(result));

        _minigameUi.ShowLevelSelectUi();
    }
}