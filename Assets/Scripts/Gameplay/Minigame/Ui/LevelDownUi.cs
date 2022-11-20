using System.Collections;
using UnityEngine;

public class LevelDownUi : MonoBehaviour
{
    [SerializeField] AudioSource _levelDownSource;
    [SerializeField] AudioClip _levelDownClip;
    [SerializeField] GameObject _description;

    InputActions.DialogueActions _actions;
    bool _skip;

    void Awake()
    {
        _actions = new InputActions().Dialogue;
        _actions.Interact.performed += _ => _skip = true;
    }

    void OnEnable()
    {
        _actions.Enable();
        _description.SetActive(false);
    }

    void OnDisable()
    {
        _actions.Disable();
    }

    public IEnumerator CO_Run()
    {
        _skip = false;
        _levelDownSource.PlayOneShot(_levelDownClip);

        var start = Time.time;
        yield return new WaitUntil(() => Time.time - start >= 2f || _skip);
        yield return null;

        _description.SetActive(true);
        yield return new WaitUntil(() => _actions.Interact.WasPressedThisFrame());
    }
}