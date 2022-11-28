using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class LoadSceneAnyKey : MonoBehaviour
{
    [SerializeField] string _scene;

    [Inject] SceneLoader _sceneLoader;

    bool _activated;

    void Update()
    {
        if (_activated) return;

        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed)
        {
            _activated = true;
            _sceneLoader.SaveAndLoadScene(_scene);
        }
    }
}