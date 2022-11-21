using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PersistentData
{
    public static int maxLevel = 97;
    public static int levelsLostPerLevel = 6;

    public Dictionary<string, StageData> stages = new();
    public Dictionary<string, bool> cinematicsDone = new();
    public Dictionary<string, bool> dialoguesDone = new();

    public int gamesBeat => stages.Values.Aggregate(
        0,
        (acc, stageData) => acc + stageData.levelsCompleted
    );

    public int level => maxLevel - Math.Max(0, gamesBeat * levelsLostPerLevel);

    public bool gauntletUnlocked => IsCinematicDone("Games8");

    public bool nightmareUnlocked => gamesBeat >= 12;

    public bool IsCinematicDone(string key)
    {
        return cinematicsDone.TryGetValue(key, out var done) && done;
    }

    public bool IsDialogueDone(string key)
    {
        return dialoguesDone.TryGetValue(key, out var done) && done;
    }

    public StageData GetStageData(string stageId)
    {
        if (stages.TryGetValue(stageId, out var data))
            return data;

        data = new StageData();
        stages.Add(stageId, data);

        return data;
    }
}

[Serializable]
public class Vector3Json
{
    public float x;
    public float y;
    public float z;

    public Vector3Json(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}