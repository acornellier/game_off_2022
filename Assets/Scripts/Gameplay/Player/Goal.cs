using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        var allGoals = FindObjectsOfType<Goal>();
        if (allGoals.Length > 1)
        {
            Utilities.DestroyGameObject(gameObject);
            return;
        }

        FindObjectOfType<PrincessGame>().GoalsFound();
    }
}