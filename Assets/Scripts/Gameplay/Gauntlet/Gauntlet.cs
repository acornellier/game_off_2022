using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] List<Minigame> _games;
    [SerializeField] InGameUi _inGameUi;
    [SerializeField] GameObject _resultsUi;
    [SerializeField] TMP_Text _resultsText;

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
        ShowResults();
        for (var i = 3; i > 0; --i)
        {
            _resultsText.text = $"Get ready!\n{i}...";
            yield return new WaitForSeconds(1);
        }

        foreach (var minigame in _games)
        {
            ShowInGameUi();

            yield return StartCoroutine(CO_RunGame(minigame));

            ShowResults();

            if (_failed)
            {
                _resultsText.text = "Too slow.\nRestarting...";
                yield return new WaitForSeconds(3);
                break;
            }

            for (var i = 2; i > 0; --i)
            {
                _resultsText.text = $"Success!\n{i}...";
                yield return new WaitForSeconds(1);
            }
        }

        if (_failed)
            RunAll();
    }

    IEnumerator CO_RunGame(Minigame minigamePrefab)
    {
        var minigame = Instantiate(minigamePrefab);

        _timeRemaining = minigame.maxTime;

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