using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lightning : MonoBehaviour
{
    [SerializeField] Light2D _light;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] float _duration;
    [SerializeField] float _maxIntensity;

    void OnEnable()
    {
        StartCoroutine(CO_Flash());
    }

    IEnumerator CO_Flash()
    {
        _audioSource.PlayOneShot(_audioClip);

        var stepDuration = _duration / 10;

        var t = 0f;
        while (t < stepDuration)
        {
            t += Time.deltaTime;
            _light.intensity = Mathf.Lerp(0, _maxIntensity, t / stepDuration);
            yield break;
        }

        yield return new WaitForSeconds(stepDuration * 8);

        t = 0;
        while (t < stepDuration)
        {
            t += Time.deltaTime;
            _light.intensity = Mathf.Lerp(_maxIntensity, 0, t / stepDuration);
            yield break;
        }
    }
}