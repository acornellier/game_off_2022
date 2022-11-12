using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FiredGame : Minigame
{
    [SerializeField] TopDownPlayer _player;
    [SerializeField] TMP_Text _firedCounter;
    [SerializeField] Target _targetPrefab;

    [SerializeField] int _numberToFire;

    [SerializeField] Vector2 _respawnRange;
    [SerializeField] Vector2 _yRange;
    [SerializeField] Vector2 _speedRange;

    float _screenHalfWidth;
    float _timeUntilSpawn;
    int _numberFired;

    void Start()
    {
        _screenHalfWidth = new Vector2(
            Camera.main.aspect * Camera.main.orthographicSize,
            Camera.main.orthographicSize
        ).x;

        _numberFired = 0;
        UpdateFiredCounter();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;
        if (_timeUntilSpawn > 0)
            return;

        var isLeft = Random.value > 0.5f;
        var x = _screenHalfWidth * (isLeft ? -1 : 1);
        var y = Random.Range(_yRange.x, _yRange.y);
        var direction = isLeft ? Vector3.right : Vector3.left;
        var speed = Random.Range(_speedRange.x, _speedRange.y);

        var target = Instantiate(_targetPrefab, new Vector3(x, y), Quaternion.identity);
        target.OnCreated(direction, speed);

        _timeUntilSpawn = Random.Range(_respawnRange.x, _respawnRange.y);
    }

    public override void Begin()
    {
        _player.EnableControls();
    }

    public override void End()
    {
        _player.DisableControls();
    }

    public void OnFired(int firedCount)
    {
        _numberFired += firedCount;
        UpdateFiredCounter();

        if (_numberFired >= _numberToFire)
            isDone = true;
    }

    void UpdateFiredCounter()
    {
        _firedCounter.text = $"Fired: {_numberFired}/{_numberToFire}";
    }
}