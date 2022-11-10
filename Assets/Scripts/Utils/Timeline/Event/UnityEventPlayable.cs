using UnityEngine.Events;
using UnityEngine.Playables;

internal class UnityEventPlayable : PlayableBehaviour
{
    UnityEvent _unityEvent;

    void Initialize(UnityEvent aUnityEvent)
    {
        _unityEvent = aUnityEvent;
    }

    void OnTrigger()
    {
        _unityEvent?.Invoke();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (info.evaluationType == FrameData.EvaluationType.Playback) // ignore scrubs
            OnTrigger();
    }

    public static Playable Create(PlayableGraph graph, UnityEvent aUnityEvent)
    {
        var scriptPlayable = ScriptPlayable<UnityEventPlayable>.Create(graph);
        scriptPlayable.GetBehaviour().Initialize(aUnityEvent);
        return scriptPlayable;
    }
}