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
        else if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main");
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            var noneLeftTo2 = true;
            foreach (var stage in new[] { "Fishing", "Packing", "Breakout", "Fired", })
            {
                var stageData = _persistentDataManager.data.GetStageData(stage);
                if (stageData.maxLevelIndexCompleted < 2)
                {
                    _persistentDataManager.data.GetStageData(stage).maxLevelIndexCompleted = 2;
                    noneLeftTo2 = false;
                    break;
                }
            }

            if (noneLeftTo2)
                foreach (var stage in new[] { "Fishing", "Packing", "Breakout", "Fired", })
                {
                    _persistentDataManager.data.GetStageData(stage).maxLevelIndexCompleted = 3;
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