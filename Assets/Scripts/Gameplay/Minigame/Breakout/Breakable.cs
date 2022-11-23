using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] ParticleSystem dustFX;
    BreakoutGame gameRules;
    Collider2D brickCollider;
    SpriteRenderer brickRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        gameRules = FindObjectOfType<BreakoutGame>();
        brickCollider = GetComponent<Collider2D>();
        brickRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<BreakoutBall>())
        {
            brickCollider.enabled = false;
            brickRenderer.enabled = false;
            dustFX.Play();
            gameRules.bricksLeft--;
            StartCoroutine(PlayDustFX());
        }
    }

    private IEnumerator PlayDustFX()
    {
        yield return new WaitForSeconds(dustFX.main.duration);
        Destroy(gameObject);
    }
}
