using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public bool isDone { get; protected set; }

    public abstract string gameName { get; }

    public abstract void Begin();
    public abstract void End();
}