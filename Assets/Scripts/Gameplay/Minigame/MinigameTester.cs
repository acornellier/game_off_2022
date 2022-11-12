using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTester : MonoBehaviour
{
    [SerializeField] TMP_Text _timer;
    [SerializeField] Image _timerFill;

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
        _timerFill.fillAmount = _timeRemaining / _minigame.maxTime;
        _timer.text = Mathf.CeilToInt(_timeRemaining).ToString();
    }
}