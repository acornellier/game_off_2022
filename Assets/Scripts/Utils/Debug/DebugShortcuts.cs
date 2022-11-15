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
        else if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            foreach (var stage in new[] { "Fishing", "Packing", "Breakout", })
            {
                _persistentDataManager.data.GetStageData(stage).maxLevelIndexCompleted += 1;
            }

            _persistentDataManager.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Keyboard.current.xKey.isPressed)
        {
            foreach (var cinematicRunner in FindObjectsOfType<CinematicRunner>())
            {
                cinematicRunner.Skip();
            }
        }
    }
}