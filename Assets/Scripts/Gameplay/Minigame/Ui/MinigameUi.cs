using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MinigameUi : MonoBehaviour
{
    [SerializeField] LevelSelectUi _levelSelectUi;
    [SerializeField] InGameUi _inGameUi;
    [SerializeField] ResultsUi _resultsUi;
    [SerializeField] LevelDownUi _levelDownUi;

    void Awake()
    {
        HideAllUis();
        _levelSelectUi.gameObject.SetActive(true);
    }

    public void ShowLevelSelectUi()
    {
        HideAllUis();
        _levelSelectUi.gameObject.SetActive(true);
        _levelSelectUi.Initialize();
    }

    public void ShowInGameUi(float maxTime)
    {
        HideAllUis();
        _inGameUi.gameObject.SetActive(true);
        SetTimeRemaining(maxTime, maxTime);
    }

    public IEnumerator ShowResults(MinigameResult result)
    {
        HideAllUis();
        _resultsUi.gameObject.SetActive(true);
        _resultsUi.SetText(result.success ? "Success!" : "Too slow");
        _resultsUi.PlaySound(result.success);
        yield return new WaitForSeconds(2);

        if (result.firstSuccess)
        {
            HideAllUis();
            _levelDownUi.gameObject.SetActive(true);
            yield return StartCoroutine(_levelDownUi.CO_Run());
        }
    }

    public void SetTimeRemaining(float timeRemaining, float maxTime)
    {
        _inGameUi.SetTimeRemaining(timeRemaining, maxTime);
    }

    void HideAllUis()
    {
        _levelSelectUi.gameObject.SetActive(false);
        _inGameUi.gameObject.SetActive(false);
        _resultsUi.gameObject.SetActive(false);
        _levelDownUi.gameObject.SetActive(false);
    }
}