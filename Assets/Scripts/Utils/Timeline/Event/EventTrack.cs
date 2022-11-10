using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

[TrackColor(186f / 255f, 193f / 255f, 43f / 255f)]
[TrackClipType(typeof(EventClip))]
public class EventTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph,
        GameObject go,
        int inputCount)
    {
        // hack to make the event names appear on the clips
        foreach (var marker in GetClips())
        {
            var asset = marker.asset as EventClip;
            if (asset == null)
                continue;
            marker.displayName = string.IsNullOrEmpty(asset.eventName)
                ? "<<Empty>>"
                : asset.eventName;
        }

        return base.CreateTrackMixer(graph, go, inputCount);
    }
}