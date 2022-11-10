using UnityEngine;
using UnityEngine.Playables;

public class DialogueAsset : PlayableAsset
{
    [SerializeField] Dialogue[] dialogues;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var dialogueManager = FindObjectOfType<DialogueManager>();

        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.dialogues = dialogues;
        behaviour.dialogueManager = dialogueManager;

        return playable;
    }
}