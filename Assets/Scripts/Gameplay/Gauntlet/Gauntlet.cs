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

    void Start()
    {
        RunAll();
    }

    void RunAll()
    {
        _failed = false;
        StartCoroutine(CO_RunAll());
    }

    IEnumerator CO_RunAll()
    {
        for (var i = 0; i < _games.Count; ++i)
        {
            yield return StartCoroutine(CO_RunGame(_games[i], i == 0));

            if (_failed)
            {
                ShowResults();
                _resultsUi.PlaySound(false);
                _resultsUi.SetText("Too slow.\nRestarting...");
                yield return new WaitForSeconds(3);
                break;
            }
        }

        if (_failed)
            RunAll();
        else
            _sceneLoader.SaveAndLoadScene("End");
    }

    IEnumerator CO_RunGame(Minigame minigamePrefab, bool isFirst)
    {
        var minigame = Instantiate(minigamePrefab);

        _inGameUi.gameObject.SetActive(true);
        _resultsUi.gameObject.SetActive(true);
        _timeRemaining = minigame.maxTime;
        _inGameUi.SetTimeRemaining(_timeRemaining, minigame.maxTime);

        if (!isFirst)
        {
            _resultsUi.PlaySound(true);
            _resultsUi.SetText("Success!");
            yield return new WaitForSeconds(2);
        }

        for (var i = 2; i > 0; --i)
        {
            _resultsUi.SetText($"Get ready!\n{i}...");
            yield return new WaitForSeconds(1);
        }

        _resultsUi.gameObject.SetActive(false);

        minigame.Begin();

        yield return new WaitUntil(
            () =>
            {
                // TODO: remove debug
                if (Keyboard.current.pKey.isPressed)
                    return true;

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
        Utilities.DestroyGameObject(minigame.gameObject);
        minigame = null;

        _failed = _timeRemaining <= 0;
    }

    void ShowResults()
    {
        _inGameUi.gameObject.SetActive(false);
        _resultsUi.gameObject.SetActive(true);
    }

    void ShowInGameUi()
    {
        _inGameUi.gameObject.SetActive(true);
        _resultsUi.gameObject.SetActive(false);
    }
}