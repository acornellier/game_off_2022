﻿using UnityEngine;

public static class Utilities
{
    public static void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public static void DestroyGameObject(GameObject go)
    {
        Object.Destroy(go);
    }
}