using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    [Inject] FadeOverlay _fadeOverlay;
    [Inject] MusicPlayer _musicPlayer;
    [Inject] PersistentDataManager _persistentDataManager;

    [SerializeField] float fadeTime = 1f;

    void Start()
    {
        _fadeOverlay.SetBlack();

        StartCoroutine(_fadeOverlay.FadeToWhite());
    }

    public void SaveAndLoadScene(string scene)
    {
        _persistentDataManager.Save();
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(string scene)
    {
        StartCoroutine(_musicPlayer.FadeOut(fadeTime));
        StartCoroutine(_fadeOverlay.FadeToBlack(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(scene);
    }
}