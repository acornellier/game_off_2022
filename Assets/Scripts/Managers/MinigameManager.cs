﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] MinigameUi _minigameUi;

    [Inject] PersistentDataManager _persistentDataManager;
    [Inject] Stage _stage;

    Coroutine _minigameCoroutine;
    Minigame _minigame;
    float _timeRemaining;

    public void StartGame(int levelIndex)
    {
        _minigameCoroutine = StartCoroutine(CO_StartGame(levelIndex));
    }

    public void BackToLevelSelect()
    {
        if (_minigameCoroutine != null)
            StopCoroutine(_minigameCoroutine);

        if (_minigame)
            EndMinigame();

        _minigameUi.ShowLevelSelectUi();
    }

    IEnumerator CO_StartGame(int levelIndex)
    {
        _minigame = Instantiate(_stage.levels[levelIndex]);

        _timeRemaining = _minigame.maxTime;

        _minigameUi.ShowInGameUi(_minigame.maxTime);
        _minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                // TODO: remove debug
                if (Keyboard.current.pKey.isPressed)
                    return true;

                if (_minigame.isDone)
                    return true;

                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                    return true;

                _minigameUi.SetTimeRemaining(_timeRemaining, _minigame.maxTime);
                return false;
            }
        );

        var success = _timeRemaining > 0;
        var stageData = _persistentDataManager.data.GetStageData(_stage.id);
        var result = new MinigameResult
        {
            maxTime = _minigame.maxTime,
            timeRemaining = _timeRemaining,
            success = success,
            firstSuccess = success && levelIndex > stageData.maxLevelIndexCompleted,
        };

        if (result.firstSuccess)
            stageData.maxLevelIndexCompleted = levelIndex;

        EndMinigame();

        yield return StartCoroutine(_minigameUi.ShowSummary(result));

        _minigameUi.ShowLevelSelectUi();
    }

    void EndMinigame()
    {
        _minigame.End();
        Utilities.DestroyGameObject(_minigame.gameObject);
        _minigame = null;
    }
}