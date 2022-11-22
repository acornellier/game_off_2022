using System.Collections;
using UnityEngine;

public class GlobalSounds : MonoBehaviour
{
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _soundSource;

    public void PlayOneShotSound(AudioClip clip)
    {
        // hacky stuff to avoid sounds right after scene load
        if (Time.timeSinceLevelLoad < 0.1f)
            return;

        _soundSource.PlayOneShot(clip);
    }

    public IEnumerator FadeOut(float fadeOutTime)
    {
        var initialVolume = _musicSource.volume;
        var t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(initialVolume, 0, t / fadeOutTime);
            yield return null;
        }
    }
}