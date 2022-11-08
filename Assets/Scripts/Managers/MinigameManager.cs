using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MinigameManager : MonoBehaviour, IPersistableData
{
    [Inject] MinigameUi _minigameUi;

    float _timeRemaining;
    float _lastTimeRemaining;
    int _totalLevelsLost;

    readonly List<MinigameResult> _results = new();

    public void Load(PersistentData data)
    {
    }

    public void Save(PersistentData data)
    {
        var scene = SceneManager.GetActiveScene().name;
        var stage = scene[5..];
        var stageData = data.GetStageData(stage);
        stageData.complete = true;
        stageData.levelsLost = TotalLevelsLost();
    }

    public IEnumerator StartGame(Minigame minigame, MinigameSettings settings)
    {
        _timeRemaining = settings.maxTime;
        minigame.gameObject.SetActive(true);

        _minigameUi.ShowTitle(minigame.gameName);
        yield return new WaitForSeconds(2);

        _minigameUi.ShowInGameUi(settings.maxTime);
        minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                if (minigame.isDone)
                    return true;

                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                    return true;

                _minigameUi.SetTimeRemaining(_timeRemaining, settings.maxTime);
                return false;
            }
        );

        minigame.End();

        var fracTime = _timeRemaining / settings.maxTime;
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
            settings = settings,
            timeRemaining = _timeRemaining,
            levelsLost = levelsLost,
        };
        _results.Add(result);

        minigame.gameObject.SetActive(false);

        yield return StartCoroutine(_minigameUi.ShowSummary(result));
    }

    int TotalLevelsLost()
    {
        return _results.Sum(result => result.levelsLost);
    }
}