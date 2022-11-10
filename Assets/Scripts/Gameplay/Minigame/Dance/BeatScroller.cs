using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [SerializeField] float _tempo;
    [SerializeField] AudioSource _musicSource;

    Vector3 _startingPosition;
    bool _isStarted;
    double _trackStartTime;

    int _lastBeat;

    void Awake()
    {
        _startingPosition = transform.position;
    }

    void Update()
    {
        if (Input.anyKeyDown && !_isStarted)
        {
            _isStarted = true;
            _trackStartTime = AudioSettings.dspTime;
            _musicSource.PlayScheduled(_trackStartTime);
        }

        if (!_isStarted)
            return;

        var beat = GetBeat();
        transform.position = _startingPosition + (float)beat * Vector3.down;

        if (beat >= _lastBeat + 1)
            d.log("beat", beat, GetTrackTime());

        _lastBeat = (int)beat;
    }

    double GetBeat()
    {
        return _tempo / 60 * GetTrackTime() - 0.3;
    }

    double GetTrackTime()
    {
        return AudioSettings.dspTime - _trackStartTime;
    }
}