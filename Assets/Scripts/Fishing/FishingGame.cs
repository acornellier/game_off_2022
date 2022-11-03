using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishingGame : MonoBehaviour
{
    [SerializeField] Collider2D _fishingArea;

    [Header("Fish")] [SerializeField] Collider2D _fish;
    [SerializeField] float _smoothMotion = 3f;
    [SerializeField] float _maxTimeInterval = 3f;
    [SerializeField] float _maxSpeed = 1f;

    [Header("Hook")] [SerializeField] Collider2D _hook;
    [SerializeField] float _hookSpeed = 0.1f;
    [SerializeField] float _hookGravity = .05f;

    [Header("Progress")] [SerializeField] Image _progressImage;
    [SerializeField] float _hookPower = 0.5f;
    [SerializeField] float _progressBarDecay = 0.1f;

    Vector3 _bottomBounds;
    Vector3 _topBounds;

    float _fishPosition;
    float _fishTargetPosition = 0.5f;
    float _fishTimer = 1f;
    float _fishSpeed;

    float _hookPosition;
    float _hookPullVelocity;

    float _progress;

    void Awake()
    {
        _bottomBounds = new Vector2(_fishingArea.bounds.center.x, _fishingArea.bounds.min.y + 1);
        _topBounds = new Vector2(_fishingArea.bounds.center.x, _fishingArea.bounds.max.y - 1);
    }

    void FixedUpdate()
    {
        MoveFish();
        MoveHook();
        CheckProgress();
    }

    void MoveFish()
    {
        _fishTimer -= Time.fixedDeltaTime;

        if (_fishTimer < 0)
        {
            _fishTimer = 1 + Random.value * _maxTimeInterval;

            while (Mathf.Abs(_fishPosition - _fishTargetPosition) < 0.2f)
            {
                _fishTargetPosition = Random.value;
            }
        }

        _fishPosition = Mathf.SmoothDamp(
            _fishPosition,
            _fishTargetPosition,
            ref _fishSpeed,
            _smoothMotion,
            _maxSpeed,
            Time.fixedDeltaTime
        );

        _fish.transform.position = Vector3.Lerp(_bottomBounds, _topBounds, _fishPosition);
    }

    void MoveHook()
    {
        if (Input.GetMouseButton(0))
            _hookPullVelocity += _hookSpeed * Time.fixedDeltaTime;

        _hookPullVelocity -= _hookGravity * Time.fixedDeltaTime;
        _hookPosition += _hookPullVelocity;
        _hookPosition = Mathf.Clamp01(_hookPosition);

        if (_hookPosition is <= 0 or >= 1)
            _hookPullVelocity = 0;

        _hook.transform.position = Vector3.Lerp(_bottomBounds, _topBounds, _hookPosition);
    }

    void CheckProgress()
    {
        if (_fish.bounds.Intersects(_hook.bounds))
        {
            _progress += _hookPower * Time.fixedDeltaTime;

            if (_progress >= 1)
                Debug.Log("YOU WIN!");
        }
        else
        {
            _progress -= _progressBarDecay * Time.fixedDeltaTime;
        }

        _progressImage.fillAmount = _progress;
    }
}