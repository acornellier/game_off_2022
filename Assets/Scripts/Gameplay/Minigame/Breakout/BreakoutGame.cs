using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

public class BreakoutGame : Minigame
{
    [Header("Test Settings")]
    [SerializeField]
    bool playgroundMode = false;

    [Header("Malafor")] [SerializeField] public GameObject paddle;
    [SerializeField] float moveSpeed;
    [SerializeField] float castDuration = .3f;
    //[SerializeField] Animation malaforIdle;
    //[SerializeField] Animation malaforCast;

    [Header("Ball")] [SerializeField] public GameObject ballPrefab;
    [SerializeField] public float ballStartSpeed;
    [SerializeField] public float ballMaxSpeed;
    [SerializeField] public float ballMinSpeed;
    [SerializeField] public float minBallAngle;

    [SerializeField] List<Slider> ammoCount;

    // [SerializeField] bool rechargable = true;
    [SerializeField] float rechargeTime = 1f;

    [SerializeField]
    [Tooltip("Distance above paddle to spawn")]
    float ballStartY;

    [Header("Bricks")] public int bricksLeft = 0;
     public Breakable[] breakables;

    EdgeCollider2D screenEdge;

    Rigidbody2D paddleBody;
    float paddleHalfWidth;
    Collider2D paddleCollider;
    SpriteRenderer malaforRenderer;
    Animator malaforAnimator;

    bool _running;
    bool firing = false;
    public int currentBalls;
    public int activeBalls;
    BreakoutBall heldBall;
    GameObject heldBallObject;
    
    [SerializeField] AudioSource _fireballSource;
    [SerializeField] AudioClip _fireballClip;

    void Awake()
    {
        if (playgroundMode)
            _running = true;
        paddleBody = paddle.GetComponent<Rigidbody2D>();
        paddleHalfWidth = paddle.GetComponentInChildren<Renderer>().bounds.size.x / 4;
        paddleCollider = paddle.GetComponent<Collider2D>();
        screenEdge = GetComponent<EdgeCollider2D>();
        var screenPoints = GenerateCameraBounds();
        screenEdge.points = screenPoints;
        malaforRenderer = paddle.GetComponentInChildren<SpriteRenderer>();
        malaforAnimator = paddle.GetComponentInChildren<Animator>();
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
        if (!_running)
            return;
        //DebugMinigame(); //comment me out!
        FireBall();
        RechargeAmmo();
        CheckForWin();
    }

    void FixedUpdate()
    {
        if (!_running)
            return;

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
        if (mousePos.x > screenWidth - paddleHalfWidth)
            mousePos = new Vector2(
                screenWidth - paddleHalfWidth,
                mousePos.y
            );
        if (mousePos.x < -screenWidth + paddleHalfWidth)
            mousePos = new Vector2(
                -screenWidth + paddleHalfWidth,
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
        //float currentAmmo = ammoCount[0].value + ammoCount[1].value + ammoCount[2].value;
        bool inputDetected = (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);
        if (currentBalls >=1 && inputDetected && !firing)
        {
            StartCoroutine(ShootNewFireball());  
        }
    }

    private IEnumerator ShootNewFireball()
    {
        firing = true;
        malaforAnimator.Play("Breakout Cast");
        paddleCollider.enabled = false;
        heldBallObject = Instantiate(
                ballPrefab,
                new Vector2(paddle.transform.position.x, paddle.transform.position.y + ballStartY),
                Quaternion.identity,
                transform
            );
        heldBall = heldBallObject.GetComponent<BreakoutBall>();
        heldBall.myChargingFX.Play(false);
        RemoveAmmo();
        activeBalls++;
        _fireballSource.PlayOneShot(_fireballClip);
        yield return new WaitForSeconds(castDuration);
        heldBall.ballReleased = true;
        heldBall.ballBody.velocity = new Vector2(0, ballStartSpeed);
        heldBall.myChargingFX.Stop();
        heldBall.myTrailFX.Play();
        heldBall.myTrail.enabled = true;
        malaforAnimator.Play("Breakout Idle");
        paddleCollider.enabled = true;
        heldBallObject = null;
        heldBall = null;
        firing = false;
    }

    void RemoveAmmo()
    {
        if (currentBalls == ammoCount.Count)
        {
            currentBalls--;
            ammoCount[currentBalls].value = 0f;
        }
        else
        {
            currentBalls--;
            ammoCount[currentBalls].value = ammoCount[currentBalls + 1].value;
            ammoCount[currentBalls + 1].value = 0f;
        }
    }

    void RechargeAmmo()
    {
        foreach (var ammo in ammoCount)
        {
            if (currentBalls == ammoCount.IndexOf(ammo) && activeBalls < ammoCount.Count - ammoCount.IndexOf(ammo))
            {
                ammo.value += Time.deltaTime * (1 / rechargeTime);
                if (ammo.value >= 1f)
                    currentBalls++;
            }
        }
    }

    void CheckForWin()
    {
        if (bricksLeft <= 0)
        {
            //breakables = FindObjectsOfType<Breakable>();
            //bricksLeft = breakables.Length;
            //if (bricksLeft <= 0)
            //{
                if (playgroundMode)
                    return;
                isDone = true;
            //}
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