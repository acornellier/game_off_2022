using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Stage", order = 0)]
public class Stage : ScriptableObject
{
    public string id;
    public string title;
    public List<Minigame> levels;
}

[Serializable]
public class StageData
{
    public int maxLevelIndexCompleted = -1;
    public int levelsCompleted => maxLevelIndexCompleted + 1;

    public bool IsLevelUnlocked(int levelIndex, bool nightmareUnlocked)
    {
        if (levelIndex == 0) return true;
        if (levelIndex >= 3) return nightmareUnlocked;
        return maxLevelIndexCompleted >= levelIndex - 1;
    }
}