using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameUi : MonoBehaviour, IPersistableData
{
    [SerializeField] TMP_Text _levelsLostCounter;
    [SerializeField] TMP_Text _timer;
    [SerializeField] CanvasGroup _canvasGroup;

    int _levelsLost;

    public void Load(PersistentData data)
    {
    }

    public void Save(PersistentData data)
    {
        var scene = SceneManager.GetActiveScene().name;
        var stage = scene[5..];
        var stageData = data.GetStageData(stage);
        stageData.complete = true;
        stageData.levelsLost = _levelsLost;
    }

    public void SetTimeRemaining(float timeRemaining)
    {
        _timer.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
    }

    public void IncreaseLevelsLost()
    {
        _levelsLost += 1;
        _levelsLostCounter.text = $"Levels lost: {_levelsLost}";
    }

    public IEnumerator FadeToBlack()
    {
        var t = 0f;
        _canvasGroup.alpha = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t / 0.3f);
            yield return null;
        }
    }

    public IEnumerator FadeToWhite()
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, t / 0.3f);
            yield return null;
        }
    }
}