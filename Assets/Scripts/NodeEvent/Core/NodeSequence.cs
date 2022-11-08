using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeSequence : NodeEvent
{
    [SerializeField] List<NodeEvent> nodeEvents;
    [SerializeField] bool playOnStart;

    bool _debugSkip;

    void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        if (playOnStart) Run();
    }

    void OnValidate()
    {
        nodeEvents = gameObject.GetComponentsInDirectChildren<NodeEvent>()
            .Where(nodeEvent => nodeEvent.gameObject.activeInHierarchy)
            .ToList();
    }

    protected override IEnumerator CO_Run()
    {
        foreach (var nodeEvent in nodeEvents)
        {
            nodeEvent.Run();
            yield return new WaitUntil(() => IsNodeEventDone(nodeEvent));
        }
    }

    bool IsNodeEventDone(NodeEvent nodeEvent)
    {
        if (_debugSkip)
        {
            _debugSkip = false;
            return true;
        }

        return nodeEvent.isDone;
    }
}