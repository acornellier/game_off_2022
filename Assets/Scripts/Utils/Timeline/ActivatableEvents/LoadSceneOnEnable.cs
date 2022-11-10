using UnityEngine;
using Zenject;

public class LoadSceneOnEnable : MonoBehaviour
{
    [SerializeField] string _scene;

    [Inject] SceneLoader _sceneLoader;

    void OnEnable()
    {
        _sceneLoader.SaveAndLoadScene(_scene);
    }
}