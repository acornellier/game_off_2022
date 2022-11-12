using System.Collections;
using UnityEngine;

public class GlobalSounds : MonoBehaviour
{
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _soundSource;

    public void PlayOneShotSound(AudioClip clip)
    {
        _soundSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (_musicSource.clip != clip)
            StartCoroutine(CO_PlayMusic(clip, fadeOutTime));
    }

    public void PlayOneShot(AudioClip clip)
    {
        StartCoroutine(CO_PlayClip(clip));
    }

    public IEnumerator FadeOut(float fadeOutTime)
    {
        var t = 0f;
        while (_musicSource.volume > 0)
        {
            t += Time.deltaTime;
            _musicSource.volume = Mathf.Clamp01(1 - t / fadeOutTime);
            yield return null;
        }
    }

    IEnumerator CO_PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (fadeOutTime > 0)
            yield return StartCoroutine(FadeOut(fadeOutTime));

        _musicSource.clip = clip;
        _musicSource.volume = 1;
        _musicSource.Play();
    }

    IEnumerator CO_PlayClip(AudioClip clip)
    {
        _musicSource.Stop();
        _musicSource.PlayOneShot(clip);
        yield return new WaitUntil(() => !_musicSource.isPlaying);
        _musicSource.Play();
    }
}