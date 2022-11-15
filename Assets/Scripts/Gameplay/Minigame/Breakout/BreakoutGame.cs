using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BreakoutGame : Minigame
{
    [Header("Test Settings")]
    [SerializeField] bool playgroundMode = false;
    
    [Header("Malafor")] [SerializeField] public GameObject paddle;
    [SerializeField] float moveSpeed;
    [SerializeField] Sprite malaforIdle;
    [SerializeField] Sprite malaforCast;

    [Header("Ball")] [SerializeField] public GameObject ballPrefab;
    [SerializeField] public float ballStartSpeed;
    [SerializeField] public float ballMaxSpeed;
    [SerializeField] public float ballMinSpeed;
    [SerializeField] public float minBallAngle;
    [SerializeField] List<Slider> ammoCount;
    [SerializeField] bool rechargable = true;
    [SerializeField] float rechargeTime = 1f;

    [SerializeField]
    [Tooltip("Distance above paddle to spawn")]
    float ballStartY;

    [Header("Bricks")] [HideInInspector] public int bricksLeft = 0;
    [HideInInspector] public Breakable[] breakables;

    EdgeCollider2D screenEdge;

    Rigidbody2D paddleBody;
    Collider2D paddleCollider;
    SpriteRenderer malaforRenderer;

    bool _running;
    public int currentBalls;
    public int activeBalls;
    BreakoutBall heldBall;
    GameObject heldBallObject;

    void Awake()
    {
        if (playgroundMode) { _running = true; }
        paddleBody = paddle.GetComponent<Rigidbody2D>();
        paddleCollider = paddle.GetComponent<Collider2D>();
        screenEdge = GetComponent<EdgeCollider2D>();
        var screenPoints = GenerateCameraBounds();
        screenEdge.points = screenPoints;
        malaforRenderer = paddle.GetComponentInChildren<SpriteRenderer>();
        currentBalls = ammoCount.Count;
    }

    void Start()
    {
        breakables = FindObjectsOfType<Breakable>();
        bricksLeft = breakables.Length;
        activeBalls = 0;
    }

    void Update()
    {
        if (!_running) { return; }
        //DebugMinigame(); //comment me out!
        FireBall();
        RechargeAmmo();
        CheckForWin();
    }

    

    void FixedUpdate()
    {
        if (!_running) { return; }

        MovePaddle();
    }

    public override void Begin()
    {
        _running = true;
    }

    public override void End()
    {
        _running = false;
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
                transform);
            heldBall = heldBallObject.GetComponent<BreakoutBall>();
            heldBall.myChargingFX.Play(false);
            heldBall.ballCollider.enabled = false;
        }

        if (currentBalls > 0 && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            heldBall.ballCollider.enabled = true;
            heldBall.ballReleased = true;
            heldBall.ballBody.velocity = new Vector2(0, ballStartSpeed);
            heldBall.myChargingFX.Stop();
            heldBall.myTrailFX.Play();
            heldBall.myTrail.enabled = true;
            malaforRenderer.sprite = malaforIdle;
            paddleCollider.enabled = true;
            heldBallObject = null;
            activeBalls++;
            RemoveAmmo();
        }
    }

    private void RemoveAmmo()
    {
        currentBalls--;
        for (int i = currentBalls; i <= 0; i--)
        {
            if(ammoCount[i].value < 1f)
            {

            }
        }
        ammoCount[currentBalls].value = 0f;
    }

    private void RechargeAmmo()
    {
        foreach(Slider ammo in ammoCount)
        {
            if(currentBalls == ammoCount.IndexOf(ammo))
            {
                ammo.value += Time.deltaTime * (1/rechargeTime);
                if(ammo.value >= 1f)
                {
                    currentBalls++;
                }
            }
        }
    }

    private void CheckForWin()
    {
        if(bricksLeft <= 0)
        {
            if (playgroundMode) { return; }
            isDone = true;
        }
    }
    Vector2[] GenerateCameraBounds()
    {
        var halfScreenHeight = Camera.main.orthographicSize;
        var halfScreenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var bounds = new Vector2[4];
        bounds[0] = new Vector2(-halfScreenWidth, -halfScreenHeight);
        bounds[1] = new Vector2(-halfScreenWidth, halfScreenHeight);
        bounds[2] = new Vector2(halfScreenWidth, halfScreenHeight);
        bounds[3] = new Vector2(halfScreenWidth, -halfScreenHeight);
        return bounds;
    }

    void DebugMinigame()
    {
        if (Keyboard.current.rKey.isPressed)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}