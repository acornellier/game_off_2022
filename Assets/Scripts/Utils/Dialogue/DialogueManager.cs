using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueImage _topImage;
    [SerializeField] DialogueImage _bottomImage;

    public bool isActive { get; private set; }

    public Action onDialogueEnd;

    InputActions.DialogueActions _actions;

    DialogueImage _activeDialogueImage;
    Queue<Dialogue> _dialogues;

    void Awake()
    {
        _actions = new InputActions().Dialogue;
        _topImage.gameObject.SetActive(false);
        _bottomImage.gameObject.SetActive(false);
    }

    void Start()
    {
        _actions.Interact.performed += OnNextInput;
    }

    void OnDisable()
    {
        StopDialogue();
        _actions.Interact.performed -= OnNextInput;
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues)
    {
        if (isActive)
            Debug.LogError("Dialogue Manager is already active");

        isActive = true;
        _actions.Enable();

        _dialogues = new Queue<Dialogue>(dialogues);
        TypeNextLine();
    }

    public void StopDialogue()
    {
        _actions.Disable();
        if (_activeDialogueImage)
        {
            _activeDialogueImage.StopCoroutine();
            _activeDialogueImage.gameObject.SetActive(false);
            _activeDialogueImage = null;
        }

        onDialogueEnd?.Invoke();
        isActive = false;
    }

    void OnNextInput(InputAction.CallbackContext ctx)
    {
        if (_activeDialogueImage && !_activeDialogueImage.isDone)
        {
            _activeDialogueImage.SkipToEndOfLine();
            return;
        }

        TypeNextLine();
    }

    void TypeNextLine()
    {
        if (_dialogues.Count <= 0)
        {
            StopDialogue();
            return;
        }

        if (_activeDialogueImage)
            _activeDialogueImage.gameObject.SetActive(false);

        var nextDialogue = _dialogues.Dequeue();
        _activeDialogueImage = nextDialogue.topOfScreen ? _topImage : _bottomImage;
        _activeDialogueImage.gameObject.SetActive(true);
        _activeDialogueImage.TypeNextLine(nextDialogue);
    }
}