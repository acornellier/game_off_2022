using UnityEngine.SceneManagement;

public class MinigameManager
{
    static MinigameManager _instance;
    public static MinigameManager Instance => _instance;

    public static void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}