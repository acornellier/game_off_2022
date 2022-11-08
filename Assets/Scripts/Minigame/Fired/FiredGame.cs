using UnityEngine;

public class FiredGame : Minigame
{
    [SerializeField] FiredPlayer _player;
    [SerializeField] Target _targetPrefab;

    public override string gameName => "Fire the Underlings";

    void Update()
    {
    }

    public override void Begin()
    {
        _player.EnableControls();
    }

    public override void End()
    {
        _player.DisableControls();
    }
}