using TMPro;
using UnityEngine;

public class Playground : MonoBehaviour
{
    [SerializeField] TMP_Text _timer;

    Minigame _minigame;
    float _timeRemaining;

    void Start()
    {
        _minigame = FindObjectOfType<Minigame>();
        _minigame.Begin();
        _timeRemaining = _minigame.maxTime;
    }

    void Update()
    {
        if (_minigame.isDone)
        {
            _timer.text = "Done";
            return;
        }

        if (_timeRemaining <= 0)
        {
            _timer.text = "Fail";
            return;
        }

        _timeRemaining -= Time.deltaTime;
        _timer.text = Mathf.CeilToInt(_timeRemaining).ToString();
    }
}