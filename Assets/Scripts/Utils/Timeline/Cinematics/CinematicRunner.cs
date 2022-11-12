using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Zenject;

public class CinematicRunner : MonoBehaviour
{
    [SerializeField] string _cinematicKey;
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] bool _skipIfDone = true;
    [SerializeField] UnityEvent _eventOnDone;

    [Inject] PersistentDataManager _persistentDataManager;

    public void Start()
    {
        if (_skipIfDone && _persistentDataManager.data.IsCinematicDone(_cinematicKey))
            return;

        foreach (var player in FindObjectsOfType<Player>())
        {
            player.DisableControls();
        }

        _playableDirector.Play();
    }

    public void OnCinematicDone()
    {
        _persistentDataManager.data.cinematicsDone[_cinematicKey] = true;
        _persistentDataManager.Save();

        _eventOnDone?.Invoke();

        foreach (var player in FindObjectsOfType<Player>())
        {
            player.EnableControls();
        }
    }
}