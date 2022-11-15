using System;
using UnityEngine;
using Zenject;

[Serializable]
public class EggcornDialogue
{
    public int gamesDoneReq;
    public Dialogue[] dialogues;

    public string key => "Eggcorn" + gamesDoneReq;
}

[RequireComponent(typeof(Collider2D))]
public class EggcornInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] EggcornDialogue[] _dialogues;

    [Inject] DialogueManager _dialogueManager;
    [Inject] PersistentDataManager _persistentDataManager;

    public void Interact()
    {
        foreach (var dialogue in _dialogues)
        {
            if (_persistentDataManager.data.IsDialogueDone(dialogue.key) ||
                _persistentDataManager.data.gamesBeat < dialogue.gamesDoneReq)
                continue;

            _dialogueManager.StartDialogue(dialogue.dialogues);

            _persistentDataManager.data.dialoguesDone[dialogue.key] = true;
            _persistentDataManager.Save();
            return;
        }
    }
}