using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader
{
    [Inject] PersistentDataManager _persistentDataManager;

    public void LoadScene(string scene)
    {
        _persistentDataManager.Save();
        SceneManager.LoadScene(scene);
    }
}