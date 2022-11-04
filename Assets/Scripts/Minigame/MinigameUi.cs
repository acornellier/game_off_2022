using System.Collections;
using TMPro;
using UnityEngine;

public class MinigameUi : MonoBehaviour
{
    [SerializeField] TMP_Text _starCounter;
    [SerializeField] TMP_Text _timer;
    [SerializeField] CanvasGroup _canvasGroup;

    int _stars;

    public void SetTimeRemaining(float timeRemaining)
    {
        _timer.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
    }

    public void IncreaseStars()
    {
        _stars += 1;
        _starCounter.text = $"Stars: {_stars}";
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
}