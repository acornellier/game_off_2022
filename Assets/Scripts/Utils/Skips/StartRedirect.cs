using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartRedirect : MonoBehaviour
{
    [Inject] PersistentDataManager _persistentDataManager;

    public void Awake()
    {
        SceneManager.LoadScene(
            _persistentDataManager.data.IsCinematicDone("intro1") ? "Main" : "Intro"
        );
    }
}