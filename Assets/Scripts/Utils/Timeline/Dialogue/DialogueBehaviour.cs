using UnityEngine.Device;
using UnityEngine.Playables;

public class DialogueBehaviour : PlayableBehaviour
{
    public Dialogue[] dialogues;
    public DialogueManager dialogueManager;

    PlayableDirector _director;
    bool _dialogueStarted;

    public override void OnPlayableCreate(Playable playable)
    {
        _director = playable.GetGraph().GetResolver() as PlayableDirector;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!Application.isPlaying || _dialogueStarted || info.weight <= 0f)
            return;

        dialogueManager.StartDialogue(dialogues);
        _dialogueStarted = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!Application.isPlaying || !_dialogueStarted || !dialogueManager.isActive)
            return;

        dialogueManager.onDialogueEnd += OnDialogueEnd;

        if (_director && _director.playableGraph.IsValid())
            _director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    void OnDialogueEnd()
    {
        if (!Application.isPlaying)
            return;

        dialogueManager.onDialogueEnd -= OnDialogueEnd;

        if (_director && _director.playableGraph.IsValid())
            _director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}