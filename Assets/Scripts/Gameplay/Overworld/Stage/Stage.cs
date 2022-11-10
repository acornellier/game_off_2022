using System;

[Serializable]
public class Stage
{
    public string name;
    public int maxLevelRequired;
    public int maxLevelsToLose;
}

[Serializable]
public class StageData
{
    public bool complete;
    public int levelsLost;
}