using UnityEngine;

public class ThroneSit : MonoBehaviour, IInteractable
{
    [SerializeField] Transform _sitSpot;
    [SerializeField] Transform _exitSpot;

    public void Interact()
    {
        var player = FindObjectOfType<TopDownPlayer>();
        player.SitThrone(_sitSpot, _exitSpot);
    }
}