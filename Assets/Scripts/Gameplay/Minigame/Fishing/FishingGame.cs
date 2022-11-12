using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishingGame : Minigame
{
    [Header("Settings")] [SerializeField] float _moveDuration = 1f;
    [SerializeField] float _maxSpeed = 1f;
    [SerializeField] Vector2 _timeBetweenMoves = Vector2.one;
    [SerializeField] Vector2 _moveDistance = new(0.1f, 1);
    [SerializeField] float _progressIncreaseRate = 0.3f;
    [SerializeField] float _progressDecayRate = 0.3f;

    [Header("Constants")] [SerializeField] Transform _topMarker;
    [SerializeField] Transform _bottomMarker;
    [SerializeField] Collider2D _fish;
    [SerializeField] Hook _hook;
    [SerializeField] Image _progressImage;

    [SerializeField] AudioSource _constantReelSource;
    [SerializeField] AudioSource _activeReelSource;

    bool _running;

    float _fishHalfHeight;
    float _hookHalfHeight;

    float _fishPosition;
    float _fishTargetPosition = 0.5f;
    float _fishTimer = 1f;
    float _fishSpeed;

    float _progress;

    void Start()
    {
        _fishHalfHeight = _fish.bounds.size.y / 2;
        _hookHalfHeight = _hook.bounds.size.y / 2;
    }

    void FixedUpdate()
    {
        if (!_running) return;

        MoveFish();
        MoveHook();
        CheckProgress();
    }

    public override void Begin()
    {
        _running = true;
        _constantReelSource.Play();
    }

    public override void End()
    {
        _running = false;
        _constantReelSource.Stop();
        _activeReelSource.Stop();
    }

    void MoveFish()
    {
        _fishTimer -= Time.fixedDeltaTime;

        if (_fishTimer < 0)
        {
            _fishTimer = Random.Range(_timeBetweenMoves.x, _timeBetweenMoves.y);

            while (Mathf.Abs(_fishPosition - _fishTargetPosition) < 0.2f)
            {
                var direction = Mathf.Sign(Random.value - _fishTargetPosition);
                var distance = Random.Range(_moveDistance.x, _moveDistance.y);
                _fishTargetPosition = Mathf.Clamp01(distance * direction);
            }
        }

        _fishPosition = Mathf.SmoothDamp(
            _fishPosition,
            _fishTargetPosition,
            ref _fishSpeed,
            _moveDuration,
            _maxSpeed,
            Time.fixedDeltaTime
        );

        _fish.transform.position = Vector3.Lerp(
            _bottomMarker.position + _fishHalfHeight * Vector3.up,
            _topMarker.position - _fishHalfHeight * Vector3.up,
            _fishPosition
        );
    }

    void MoveHook()
    {
        var position = _hook.GetNewPosition();

        _hook.transform.position = Vector3.Lerp(
            _bottomMarker.position + _hookHalfHeight * Vector3.up,
            _topMarker.position - _hookHalfHeight * Vector3.up,
            position
        );
    }

    void CheckProgress()
    {
        if (_fish.bounds.Intersects(_hook.bounds))
        {
            _progress += _progressIncreaseRate * Time.fixedDeltaTime;
            _hook.SetActive(true);
            _activeReelSource.Play();

            if (_progress >= 1)
                isDone = true;
        }
        else
        {
            _progress -= _progressDecayRate * Time.fixedDeltaTime;
            _hook.SetActive(false);
            _activeReelSource.Stop();
        }

        _progress = Mathf.Clamp01(_progress);

        _progressImage.fillAmount = _progress;
    }
}