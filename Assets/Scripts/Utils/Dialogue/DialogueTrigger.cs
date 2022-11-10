using UnityEngine;
using Zenject;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] bool triggerOnStart;
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    void Start()
    {
        if (triggerOnStart) Trigger();
    }

    void Trigger()
    {
        _dialogueManager.StartDialogue(dialogues);
        gameObject.SetActive(false);
    }
}