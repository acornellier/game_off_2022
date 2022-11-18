using TMPro;
using UnityEngine;

public class ResultsUi : MonoBehaviour
{
    [SerializeField] TMP_Text _resultsText;

    [SerializeField] AudioSource _failSource;
    [SerializeField] AudioClip _failClip;
    [SerializeField] AudioSource _sucessSource;
    [SerializeField] AudioClip _successClip;

    public void PlaySound(bool success)
    {
        if (success)
            _sucessSource.PlayOneShot(_successClip);
        else
            _failSource.PlayOneShot(_failClip);
    }

    public void SetText(string text)
    {
        _resultsText.text = text;
    }
}