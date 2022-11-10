using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class EventClip : PlayableAsset
{
    public override double duration => 2;

    public string eventName = string.Empty;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        if (string.IsNullOrEmpty(eventName))
            return Playable.Create(graph);

        var eventTable = go.GetComponent<EventTable>();
        if (eventTable == null)
            return Playable.Create(graph);

        var unityEvent = eventTable.GetEvent(eventName);
        return unityEvent == null
            ? Playable.Create(graph)
            : UnityEventPlayable.Create(graph, unityEvent);
    }
}