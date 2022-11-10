using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PersistentData
{
    public static int maxLevel = 99;

    public Dictionary<string, StageData> stages = new();
    public bool introComplete;

    public StageData GetStageData(string stage)
    {
        if (stages.TryGetValue(stage, out var data))
            return data;

        data = new StageData();
        stages.Add(stage, data);

        return data;
    }

    public int level
    {
        get
        {
            return stages.Values.Aggregate(
                maxLevel,
                (current, stageData) => current - stageData.levelsLost
            );
        }
    }
}