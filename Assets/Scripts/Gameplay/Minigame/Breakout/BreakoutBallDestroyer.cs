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
            
            StartCoroutine(DelayDestroy(collision));
        }
    }

    private IEnumerator DelayDestroy(Collider2D collision)
    {
        gameRules.activeBalls--;
        yield return new WaitForSeconds(0.1f);
        Destroy(collision.gameObject);
    }
}
