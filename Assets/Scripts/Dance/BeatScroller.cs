using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [SerializeField] float _tempo;
    [SerializeField] AudioSource _musicSource;

    bool _isStarted;

    void Update()
    {
        if (Input.anyKeyDown && !_isStarted)
        {
            _isStarted = true;
            _musicSource.Play();
        }
    }

    void FixedUpdate()
    {
        if (!_isStarted)
            return;

        transform.position += _tempo / 60 * Time.fixedDeltaTime * Vector3.down;
    }
}
