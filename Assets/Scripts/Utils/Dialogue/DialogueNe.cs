using System.Collections;
using UnityEngine;
using Zenject;

public class DialogueNe : NodeEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    protected override IEnumerator CO_Run()
    {
        _dialogueManager.StartDialogue(dialogues);
        yield return new WaitUntil(() => !_dialogueManager.isActive);
    }
}