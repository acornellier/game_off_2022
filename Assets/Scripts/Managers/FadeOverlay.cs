using System.Collections;
using UnityEngine;

public class FadeOverlay : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;

    public void SetBlack()
    {
        _canvasGroup.alpha = 1;
    }

    public IEnumerator FadeToBlack(float fadeTime = 0.3f)
    {
        var t = 0f;
        _canvasGroup.alpha = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(float fadeTime = 0.3f)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
    }
}