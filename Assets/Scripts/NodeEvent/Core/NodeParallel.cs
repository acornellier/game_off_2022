using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeParallel : NodeEvent
{
    [SerializeField] List<NodeEvent> nodeEvents;
    [SerializeField] float timeBetweenStarts;

    void Awake()
    {
        OnValidate();
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
            if (timeBetweenStarts > 0)
                yield return new WaitForSeconds(timeBetweenStarts);
        }

        foreach (var nodeEvent in nodeEvents)
        {
            yield return new WaitUntil(() => nodeEvent.isDone);
        }
    }
}