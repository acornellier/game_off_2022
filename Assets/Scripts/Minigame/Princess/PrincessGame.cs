using UnityEngine;

public class PrincessGame : Minigame
{
    [SerializeField] Player _player;

    public override string gameName => "Princess In A Castle";

    bool _running;

    public override void Begin()
    {
        _player.EnableControls();
    }

    public override void End()
    {
        _player.DisableControls();
    }

    public void GoalsFound()
    {
        isDone = true;
    }
}