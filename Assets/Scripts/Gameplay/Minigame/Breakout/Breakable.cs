using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    BreakoutGame gameRules;
    
    // Start is called before the first frame update
    void Start()
    {
        gameRules = FindObjectOfType<BreakoutGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<BreakoutBall>())
        {
            gameRules.bricksBroken++;
            Destroy(gameObject);
        }
    }
}
