using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource _footstepSource;
    [SerializeField] AudioClip[] _footstepClips;

    public void Footstep()
    {
        _footstepSource.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
    }
}