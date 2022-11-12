using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugShortcuts : MonoBehaviour
{
    [Inject] PersistentDataManager _persistentDataManager;

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (Keyboard.current.shiftKey.isPressed)
                PersistentDataManager.Reset();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            _persistentDataManager.Save();
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame && Keyboard.current.shiftKey.isPressed)
        {
            foreach (var stage in new[] { "Fishing", })
            {
                _persistentDataManager.data.stages[stage] =
                    new StageData { maxLevelIndexCompleted = 99, };
            }

            _persistentDataManager.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}