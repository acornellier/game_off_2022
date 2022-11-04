using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventNe : NodeEvent
{
    [SerializeField] UnityEvent unityEvent;

    protected override IEnumerator CO_Run()
    {
        unityEvent.Invoke();
        yield break;
    }
}