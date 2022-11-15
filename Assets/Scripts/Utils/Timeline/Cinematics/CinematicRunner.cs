using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class CinematicRunner : MonoBehaviour
{
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] string _cinematicKey;
    [SerializeField] int _gamesDoneReq;
    [SerializeField] bool _skipIfDone = true;

    [Inject] DialogueManager _dialogueManager;
    [Inject] PersistentDataManager _persistentDataManager;

    bool _running;

    public void Start()
    {
        if (_skipIfDone && _persistentDataManager.data.IsCinematicDone(_cinematicKey))
            return;

        if (_persistentDataManager.data.gamesBeat < _gamesDoneReq)
            return;

        foreach (var player in FindObjectsOfType<Player>())
        {
            player.DisableControls();
        }

        _running = true;
        _playableDirector.Play();
        _playableDirector.stopped += _ => OnDirectorStopped();
    }

    public void Pause()
    {
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Resume()
    {
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void OnDirectorStopped()
    {
        _persistentDataManager.data.cinematicsDone[_cinematicKey] = true;
        _persistentDataManager.Save();

        foreach (var player in FindObjectsOfType<Player>())
        {
            player.EnableControls();
        }

        _running = false;
    }

    public void Skip()
    {
        if (!_running) return;

        _dialogueManager.StopDialogue();
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(10);
    }
}