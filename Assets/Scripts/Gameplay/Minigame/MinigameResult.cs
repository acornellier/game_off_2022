public class MinigameResult
{
    public int maxTime;
    public float timeRemaining;
    public int levelsLost;
    public bool success => levelsLost > 0;
}