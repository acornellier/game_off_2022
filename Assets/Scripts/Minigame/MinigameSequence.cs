using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameSequence : NodeEvent
{
    [SerializeField] List<MinigameNe> _minigames;
    [SerializeField] bool _playOnStart;

    bool _debugSkip;
    int _stars;

    void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        if (_playOnStart) Run();
    }

    void OnValidate()
    {
        _minigames = gameObject.GetComponentsInDirectChildren<MinigameNe>()
            .Where(nodeEvent => nodeEvent.gameObject.activeInHierarchy)
            .ToList();
    }

    protected override IEnumerator CO_Run()
    {
        foreach (var minigame in _minigames)
        {
            minigame.Run();

            yield return new WaitUntil(() => IsGameDone(minigame));
        }
    }

    bool IsGameDone(MinigameNe minigameNe)
    {
        if (_debugSkip)
        {
            _debugSkip = false;
            return true;
        }

        return minigameNe.isDone;
    }
}