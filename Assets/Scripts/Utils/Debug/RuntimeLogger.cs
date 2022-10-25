using TMPro;
using UnityEngine;

public class RuntimeLogger : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        _text.text = logString + "\n" + _text.text;
    }
}