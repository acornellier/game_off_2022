using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    [SerializeField] GameObject _timer;
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Image _timerFill;

    public void SetTimeRemaining(float timeRemaining, float maxTime)
    {
        _timer.SetActive(maxTime > 0);
        _timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        _timerFill.fillAmount = timeRemaining / maxTime;
    }
}