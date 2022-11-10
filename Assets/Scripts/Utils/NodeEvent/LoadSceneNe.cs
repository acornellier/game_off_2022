using System.Collections;
using UnityEngine;
using Zenject;

public class LoadSceneNe : NodeEvent
{
    [SerializeField] string scene;

    [Inject] SceneLoader _sceneLoader;

    protected override IEnumerator CO_Run()
    {
        _sceneLoader.LoadScene(scene);
        yield break;
    }
}