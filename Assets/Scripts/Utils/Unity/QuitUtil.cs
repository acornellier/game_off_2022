using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class QuitUtil
{
    #region Static

    public static bool isQuitting;

#if UNITY_EDITOR
    [InitializeOnEnterPlayMode]
    static void EnterPlayMode(EnterPlayModeOptions options)
    {
        isQuitting = false;
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        isQuitting = false;
        Application.quitting += Quit;
    }

    static void Quit()
    {
        isQuitting = true;
    }

    #endregion
}