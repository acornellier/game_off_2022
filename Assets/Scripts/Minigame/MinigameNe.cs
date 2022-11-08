using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public enum MinigameResult
{
    None,
    Success,
    Failure,
}

public class MinigameNe : NodeEvent
{
    [SerializeField] Minigame _minigame;
    [SerializeField] float maxTime;

    public MinigameResult result = MinigameResult.None;

    float _timeRemaining;

    [Inject] MinigameUi _minigameUi;

    protected override IEnumerator CO_Run()
    {
        _timeRemaining = maxTime;

        _minigame.gameObject.SetActive(true);

        yield return StartCoroutine(_minigameUi.FadeToWhite());

        _minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                var done = _minigame.isDone;
                if (done)
                {
                    _minigameUi.IncreaseLevelsLost();
                    result = MinigameResult.Success;
                    return true;
                }

                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                {
                    result = MinigameResult.Failure;
                    return true;
                }

                _minigameUi.SetTimeRemaining(_timeRemaining);
                return false;
            }
        );

        _minigame.End();

        yield return StartCoroutine(_minigameUi.FadeToBlack());

        _minigame.gameObject.SetActive(false);
    }
}