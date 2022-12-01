using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] List<Minigame> _games;
    [SerializeField] InGameUi _inGameUi;
    [SerializeField] ResultsUi _resultsUi;

    [Inject] SceneLoader _sceneLoader;

    float _timeRemaining;
    bool _failed;
    int _checkpoint;

    void Awake()
    {
        RunFromCheckpoint();
    }

    void RunFromCheckpoint()
    {
        _failed = false;
        StartCoroutine(CO_RunFromCheckpoint());
    }

    IEnumerator CO_RunFromCheckpoint()
    {
        for (var i = _checkpoint; i < _games.Count; ++i)
        {
            yield return StartCoroutine(CO_RunGame(i));
            if (_failed)
            {
                RunFromCheckpoint();
                yield break;
            }
        }

        _sceneLoader.SaveAndLoadScene("End");
    }

    IEnumerator CO_RunGame(int gameIndex)
    {
        var minigamePrefab = _games[gameIndex];
        var minigame = Instantiate(minigamePrefab);

        _inGameUi.gameObject.SetActive(true);
        _resultsUi.gameObject.SetActive(true);
        _timeRemaining = minigame.maxTime;
        _inGameUi.SetTimeRemaining(_timeRemaining, minigame.maxTime);

        _resultsUi.SetText("Get ready...");
        yield return new WaitForSeconds(2);

        _resultsUi.gameObject.SetActive(false);

        minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                if (minigame.isDone)
                    return true;

                _timeRemaining -= Time.deltaTime;
                if (_timeRemaining <= 0)
                    return true;

                _inGameUi.SetTimeRemaining(_timeRemaining, minigame.maxTime);
                return false;
            }
        );

        minigame.End();
        _failed = _timeRemaining <= 0;


        _inGameUi.gameObject.SetActive(false);
        _resultsUi.gameObject.SetActive(true);
        if (_failed)
        {
            _resultsUi.PlaySound(false);
            _resultsUi.SetText("Too slow.\nRestarting...");
            yield return new WaitForSeconds(3);
        }
        else
        {
            _resultsUi.PlaySound(true);
            _resultsUi.SetText("Success!");
            yield return new WaitForSeconds(1.5f);

            if (gameIndex is 3)
            {
                _checkpoint = gameIndex + 1;
                _resultsUi.SetText("Checkpoint reached!");
                yield return new WaitForSeconds(2);
            }
        }

        Utilities.DestroyGameObject(minigame.gameObject);
        minigame = null;
    }
}