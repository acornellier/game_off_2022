using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BreakoutGame : Minigame
{
    [Header("Malafor")] [SerializeField] public GameObject paddle;
    [SerializeField] float moveSpeed;
    [SerializeField] Sprite malaforIdle;
    [SerializeField] Sprite malaforCast;

    [Header("Ball")] [SerializeField] public GameObject ballPrefab;
    [SerializeField] public float ballStartSpeed;
    [SerializeField] public float ballMaxSpeed;
    [SerializeField] public float ballMinSpeed;
    [SerializeField] public float minBallAngle;
    [SerializeField] int startingBalls = 3;

    [SerializeField]
    [Tooltip("Distance above paddle to spawn")]
    float ballStartY;

    [Header("Bricks")] [SerializeField] int bricksToWin;
    [HideInInspector] public int bricksBroken = 0;
    [HideInInspector] public Breakable[] breakables;

    EdgeCollider2D screenEdge;

    Rigidbody2D paddleBody;
    Collider2D paddleCollider;
    SpriteRenderer malaforRenderer;

    // bool _running;
    // bool holdingBall;
    public int currentBalls;
    BreakoutBall heldBall;
    GameObject heldBallObject;

    void Awake()
    {
        paddleBody = paddle.GetComponent<Rigidbody2D>();
        paddleCollider = paddle.GetComponent<Collider2D>();
        screenEdge = GetComponent<EdgeCollider2D>();
        var screenPoints = GenerateCameraBounds();
        screenEdge.points = screenPoints;
        // holdingBall = true;
        malaforRenderer = paddle.GetComponentInChildren<SpriteRenderer>();
        currentBalls = startingBalls;
    }

    void Start()
    {
        breakables = FindObjectsOfType<Breakable>();
        if (breakables.Length < bricksToWin)
            bricksToWin = breakables.Length;
    }

    void Update()
    {
        DebugMinigame(); //comment me out!
        FireBall();
    }

    void FixedUpdate()
    {
        //if (!_running) { return; }

        MovePaddle();
    }

    public override void Begin()
    {
        // _running = true;
    }

    public override void End()
    {
        // _running = false;
    }

    void MovePaddle()
    {
        var screenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mousePos.x > screenWidth)
            mousePos = new Vector2(
                screenWidth - paddle.GetComponentInChildren<Renderer>().bounds.size.x / 2,
                mousePos.y
            );
        if (mousePos.x < -screenWidth)
            mousePos = new Vector2(
                -screenWidth + paddle.GetComponentInChildren<Renderer>().bounds.size.x / 2,
                mousePos.y
            );
        var targetPos = Vector2.MoveTowards(
            paddleBody.position,
            mousePos,
            Time.deltaTime * moveSpeed
        );
        paddleBody.MovePosition(new Vector2(targetPos.x, paddleBody.position.y));
    }

    void FireBall()
    {
        if (currentBalls > 0 && Mouse.current.leftButton.wasPressedThisFrame)
        {
            malaforRenderer.sprite = malaforCast;
            paddleCollider.enabled = false;
            heldBallObject = Instantiate(
                ballPrefab,
                new Vector2(paddle.transform.position.x, paddle.transform.position.y + ballStartY),
                Quaternion.identity,
                transform
            );
            heldBall = heldBallObject.GetComponent<BreakoutBall>();
            heldBall.myChargingFX.Play(false);
            heldBall.ballCollider.enabled = false;
        }

        if (currentBalls > 0 && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            // holdingBall = false;
            heldBall.ballCollider.enabled = true;
            heldBall.ballReleased = true;
            heldBall.ballBody.velocity = new Vector2(0, ballStartSpeed);
            heldBall.myChargingFX.Stop();
            heldBall.myTrailFX.Play();
            heldBall.myTrail.enabled = true;
            malaforRenderer.sprite = malaforIdle;
            paddleCollider.enabled = true;
            heldBallObject = null;
            currentBalls--;
        }
    }

    Vector2[] GenerateCameraBounds()
    {
        var halfScreenHeight = Camera.main.orthographicSize;
        var halfScreenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var bounds = new Vector2[5];
        bounds[0] = new Vector2(-halfScreenWidth, halfScreenHeight);
        bounds[1] = new Vector2(halfScreenWidth, halfScreenHeight);
        bounds[2] = new Vector2(halfScreenWidth, -halfScreenHeight);
        bounds[3] = new Vector2(-halfScreenWidth, -halfScreenHeight);
        bounds[4] = new Vector2(-halfScreenWidth, halfScreenHeight);
        return bounds;
    }

    void DebugMinigame()
    {
        if (Keyboard.current.rKey.isPressed)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}