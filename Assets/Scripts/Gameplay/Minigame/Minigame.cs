using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public int maxTime;

    public bool isDone { get; protected set; }

    public abstract void Begin();
    public abstract void End();
}