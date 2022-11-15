using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBallDestroyer : MonoBehaviour
{
    [SerializeField] BreakoutGame gameRules;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BreakoutBall>())
        {
            gameRules.activeBalls--;
            Destroy(collision.gameObject);
        }
    }
}
