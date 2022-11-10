using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource _footstepSource;
    [SerializeField] AudioClip _footstepClip;

    public void Footstep()
    {
        _footstepSource.PlayOneShot(_footstepClip);
    }
}