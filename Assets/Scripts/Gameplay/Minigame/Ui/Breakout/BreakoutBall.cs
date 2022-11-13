using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBall : MonoBehaviour
{
    public bool ballReleased;

    public Rigidbody2D ballBody;
    public Collider2D ballCollider;
    BreakoutGame gameRules;
    GameObject paddle;
    public TrailRenderer myTrail;
    public ParticleSystem myChargingFX;
    [SerializeField] ParticleSystem myCollisionFX;
    [SerializeField] public ParticleSystem myTrailFX;


    // Start is called before the first frame update
    void Awake()
    {
        ballBody = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();
        gameRules = FindObjectOfType<BreakoutGame>();
        paddle = gameRules.paddle;
        ballReleased = false;
        myTrail = GetComponent<TrailRenderer>();
        myTrail.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBall();
    }

    private void Update()
    {
        ParticlesFollow();
    }

    private void ParticlesFollow()
    {
        var particles = new ParticleSystem.Particle[myChargingFX.particleCount];
        myChargingFX.GetParticles(particles);
        for (int i = 0; i < particles.GetUpperBound(0); i++)
        {
            float ForceToAdd = (particles[i].startLifetime - particles[i].remainingLifetime) * (10 * Vector3.Distance(transform.position, particles[i].position));
            particles[i].velocity = (transform.position - particles[i].position).normalized * ForceToAdd;
        }
        myChargingFX.SetParticles(particles, particles.Length);
    }

    private void MoveBall()
    {
        if (!ballReleased)
        {
            transform.position = new Vector2(paddle.transform.position.x, transform.position.y);
        }
        else
        {
            if (ballBody.velocity.y < 0f && ballBody.velocity.y > -gameRules.minBallAngle)
            {
                ballBody.velocity = new Vector2(ballBody.velocity.x, -gameRules.minBallAngle);
            }
            if (ballBody.velocity.y > 0f && ballBody.velocity.y < gameRules.minBallAngle)
            {
                ballBody.velocity = new Vector2(ballBody.velocity.x, gameRules.minBallAngle);
            }
            if (ballBody.velocity.magnitude > gameRules.ballMaxSpeed)
            {
                ballBody.velocity = ballBody.velocity.normalized * gameRules.ballMaxSpeed;
            }
            if (ballBody.velocity.magnitude < gameRules.ballMinSpeed)
            {
                ballBody.velocity = ballBody.velocity.normalized * gameRules.ballMinSpeed;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        myCollisionFX.Play();
    }

}
