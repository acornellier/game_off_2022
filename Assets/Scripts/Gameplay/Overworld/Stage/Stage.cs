using System;
using UnityEngine.Serialization;

[Serializable]
public class Stage
{
    [FormerlySerializedAs("name")] public string id;
    public string title;
    public int maxLevelRequired;
    public int maxLevelsToLose;
}

[Serializable]
public class StageData
{
    public bool complete;
    public int levelsLost;
}