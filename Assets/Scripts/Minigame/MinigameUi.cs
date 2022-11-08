using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameUi : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] GameObject _titleUi;
    [SerializeField] TMP_Text _titleText;

    [SerializeField] GameObject _inGameUi;
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Image _timerFill;

    [SerializeField] GameObject _summaryUi;
    [SerializeField] TMP_Text _levelsLostCounter;

    void Awake()
    {
        HideAllUis();
    }

    public void ShowTitle(string title)
    {
        HideAllUis();
        _titleUi.SetActive(true);
        _titleText.text = title;
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
        _levelsLostCounter.text = $"Levels lost: {result.levelsLost}";
        yield return new WaitForSeconds(2);
        _summaryUi.SetActive(false);
    }

    public void SetTimeRemaining(float timeRemaining, float maxTime)
    {
        _timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        _timerFill.fillAmount = timeRemaining / maxTime;
        print(_timerFill.fillAmount);
    }

    public IEnumerator FadeToBlack()
    {
        var t = 0f;
        _canvasGroup.alpha = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t / 0.3f);
            yield return null;
        }
    }

    public IEnumerator FadeToWhite()
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, t / 0.3f);
            yield return null;
        }
    }

    void HideAllUis()
    {
        _titleUi.SetActive(false);
        _inGameUi.SetActive(false);
        _summaryUi.SetActive(false);
    }
}