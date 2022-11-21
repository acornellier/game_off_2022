using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    [Inject] FadeOverlay _fadeOverlay;
    [Inject] GlobalSounds _globalSounds;
    [Inject] PersistentDataManager _persistentDataManager;

    [SerializeField] float fadeTime = 1f;

    void Start()
    {
        _fadeOverlay.SetBlack();

        StartCoroutine(_fadeOverlay.FadeToWhite(fadeTime));
    }

    public void SaveAndLoadScene(string scene)
    {
        _persistentDataManager.Save();
        StartCoroutine(LoadScene(scene));
    }

    public void LoadSceneInstant(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    IEnumerator LoadScene(string scene)
    {
        StartCoroutine(_globalSounds.FadeOut(fadeTime));
        StartCoroutine(_fadeOverlay.FadeToBlack(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(scene);
    }
}