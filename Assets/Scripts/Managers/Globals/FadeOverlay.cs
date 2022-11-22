using System.Collections;
using UnityEngine;

public class FadeOverlay : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;

    public void SetBlack()
    {
        _canvasGroup.alpha = 1;
    }

    public IEnumerator FadeToBlack(float fadeTime)
    {
        var t = 0f;
        _canvasGroup.alpha = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(float fadeTime)
    {
        const float breakpoint = 0.75f;
        var halfFadeTime = fadeTime / 2;

        var t = 0f;
        while (t < halfFadeTime)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, breakpoint, t / halfFadeTime);
            yield return null;
        }

        t = 0f;
        while (t < halfFadeTime)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(breakpoint, 0, t / halfFadeTime);
            yield return null;
        }
    }
}