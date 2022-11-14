using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MinigameUi : MonoBehaviour
{
    [SerializeField] LevelSelectUi _levelSelectUi;

    [SerializeField] GameObject _inGameUi;
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Image _timerFill;

    [SerializeField] GameObject _summaryUi;
    [SerializeField] TMP_Text _resultsText;

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
        _inGameUi.SetActive(true);
        SetTimeRemaining(maxTime, maxTime);
    }

    public IEnumerator ShowSummary(MinigameResult result)
    {
        HideAllUis();
        _summaryUi.SetActive(true);
        _resultsText.text = result.success ? "Success!" : "Too slow";
        yield return new WaitForSeconds(2);

        _summaryUi.SetActive(false);

        if (result.firstSuccess)
        {
            HideAllUis();
            _levelDownUi.gameObject.SetActive(true);
            yield return StartCoroutine(_levelDownUi.CO_Run());
        }
    }

    public void SetTimeRemaining(float timeRemaining, float maxTime)
    {
        _timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        _timerFill.fillAmount = timeRemaining / maxTime;
    }

    void HideAllUis()
    {
        _levelSelectUi.gameObject.SetActive(false);
        _inGameUi.SetActive(false);
        _summaryUi.SetActive(false);
        _levelDownUi.gameObject.SetActive(false);
    }
}