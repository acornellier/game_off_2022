using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class StageStart : MonoBehaviour
{
    [SerializeField] Stage _stage;
    [SerializeField] bool _uiLeft = true;

    [Inject] StageUi _stageUi;

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _stageUi.Activate(_stage, _uiLeft);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _stageUi.Deactivate();
    }
}