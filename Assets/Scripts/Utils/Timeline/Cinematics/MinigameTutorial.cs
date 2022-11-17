using UnityEngine;
using Zenject;

public class MinigameTutorial : MonoBehaviour
{
    [Inject] GlobalLight _globalLight;
    [Inject] MinigameManager _minigameManager;

    void OnEnable()
    {
        _globalLight.light.enabled = false;
        _minigameManager.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (QuitUtil.isQuitting) return;

        _globalLight.light.enabled = true;
        _minigameManager.gameObject.SetActive(true);
    }
}