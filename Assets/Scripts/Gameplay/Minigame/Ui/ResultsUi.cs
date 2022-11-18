using TMPro;
using UnityEngine;

public class ResultsUi : MonoBehaviour
{
    [SerializeField] TMP_Text _resultsText;

    [SerializeField] AudioSource _failSource;
    [SerializeField] AudioClip _failClip;
    [SerializeField] AudioSource _sucessSource;
    [SerializeField] AudioClip _successClip;

    public void ShowResult(bool success)
    {
        _resultsText.text = success ? "Success!" : "Too slow";

        if (success)
            _sucessSource.PlayOneShot(_successClip);
        else
            _failSource.PlayOneShot(_failClip);
    }
}