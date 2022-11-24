using System.Collections;
using UnityEngine;
using Zenject;

public class FireInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer fire;

    [Inject] EasterEggActivator _easterEggActivator;

    public bool isActive => fire.color.a > 0;

    Coroutine _coroutine;

    public void Interact()
    {
        if (_coroutine != null)
            return;

        _coroutine = StartCoroutine(CO_Toggle());
    }

    IEnumerator CO_Toggle()
    {
        const float fadeDuration = 1f;
        var isFading = isActive;
        var t = 0f;
        var originalColor = fire.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            var newColor = originalColor;
            newColor.a = Mathf.Lerp(originalColor.a, isFading ? 0 : 1, t / fadeDuration);
            fire.color = newColor;
            yield return null;
        }

        _easterEggActivator.Verify();
        _coroutine = null;
    }
}