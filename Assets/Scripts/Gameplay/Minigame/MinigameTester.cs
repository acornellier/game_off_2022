using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTester : MonoBehaviour
{
    [SerializeField] TMP_Text _timer;
    [SerializeField] Image _timerFill;

    Minigame _minigame;
    float _timeRemaining;
    bool _failed;

    void Start()
    {
        _minigame = FindObjectOfType<Minigame>();
        _minigame.Begin();
        _timeRemaining = _minigame.maxTime;
    }

    void Update()
    {
        if (_failed) return;

        if (_minigame.isDone)
        {
            _timer.text = "Win";
            return;
        }

        if (_timeRemaining <= 0)
        {
            _failed = true;
            _timer.text = "Fail";
            return;
        }

        _timeRemaining -= Time.deltaTime;
        _timerFill.fillAmount = _timeRemaining / _minigame.maxTime;
        _timer.text = Mathf.CeilToInt(_timeRemaining).ToString();
    }
}