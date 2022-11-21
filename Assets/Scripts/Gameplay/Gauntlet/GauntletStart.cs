using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class GauntletStart : MonoBehaviour
{
    [Inject] GauntletStartUi _gauntletStartUi;

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _gauntletStartUi.Activate();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _gauntletStartUi.Deactivate();
    }
}