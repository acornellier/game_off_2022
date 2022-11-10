using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (_source.clip != clip)
            StartCoroutine(CO_PlayMusic(clip, fadeOutTime));
    }

    public void PlayOneShot(AudioClip clip)
    {
        StartCoroutine(CO_PlayClip(clip));
    }

    public IEnumerator FadeOut(float fadeOutTime)
    {
        var t = 0f;
        while (_source.volume > 0)
        {
            t += Time.deltaTime;
            _source.volume = Mathf.Clamp01(1 - t / fadeOutTime);
            yield return null;
        }
    }

    IEnumerator CO_PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (fadeOutTime > 0)
            yield return StartCoroutine(FadeOut(fadeOutTime));

        _source.clip = clip;
        _source.volume = 1;
        _source.Play();
    }

    IEnumerator CO_PlayClip(AudioClip clip)
    {
        _source.Stop();
        _source.PlayOneShot(clip);
        yield return new WaitUntil(() => !_source.isPlaying);
        _source.Play();
    }
}