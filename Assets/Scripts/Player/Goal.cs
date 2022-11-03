using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        Debug.Log("Done!");
    }
}