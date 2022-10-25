using System;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    public bool isPaused { get; private set; }
    public event Action<bool> OnGamePausedChange;

    public void Initialize()
    {
        Time.timeScale = 1;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;

        Time.timeScale = isPaused ? 0 : 1;

        OnGamePausedChange?.Invoke(isPaused);
    }
}