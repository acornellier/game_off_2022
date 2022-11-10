using System;
using System.Collections;
using UnityEngine;
using Zenject;

[Serializable]
public class MinigameSettings
{
    public float maxTime = 10;
}

public class MinigameResult
{
    public MinigameSettings settings;
    public float timeRemaining;
    public int levelsLost;
}

public class MinigameNe : NodeEvent
{
    [SerializeField] Minigame _minigame;
    [SerializeField] MinigameSettings _settings;

    [Inject] MinigameManager _minigameManager;

    protected override IEnumerator CO_Run()
    {
        yield return StartCoroutine(_minigameManager.StartGame(_minigame, _settings));
    }
}