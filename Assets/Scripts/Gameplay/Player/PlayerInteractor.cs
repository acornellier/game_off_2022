using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    IInteractable _interactable;

    void OnTriggerEnter2D(Collider2D col)
    {
        var interactable = col.GetComponent<IInteractable>();
        if (interactable != null)
            _interactable = interactable;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var interactable = col.GetComponent<IInteractable>();
        if (interactable == _interactable)
            _interactable = null;
    }

    public void Interact()
    {
        _interactable?.Interact();
    }
}