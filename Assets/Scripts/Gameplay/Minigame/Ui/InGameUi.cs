using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Image _timerFill;

    public void SetTimeRemaining(float timeRemaining, float maxTime)
    {
        _timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        _timerFill.fillAmount = timeRemaining / maxTime;
    }
}