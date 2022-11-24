using UnityEngine;
using Zenject;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Dialogue[] _dialogues;

    [Inject] DialogueManager _dialogueManager;

    public void Interact()
    {
        _dialogueManager.StartDialogue(_dialogues);
    }
}