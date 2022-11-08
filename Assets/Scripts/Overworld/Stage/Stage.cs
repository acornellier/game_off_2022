using System;

[Serializable]
public class Stage
{
    public string name;
    public int maxPowerRequired;
    public int maxPowerToLose;
}

[Serializable]
public class StageData
{
    public bool complete;
    public int powerLost;
}