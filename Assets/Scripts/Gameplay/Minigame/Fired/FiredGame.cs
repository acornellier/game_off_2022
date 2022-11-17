using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FiredGame : Minigame
{
    [SerializeField] FiredPlayer _player;
    [SerializeField] TMP_Text _firedCounter;
    [SerializeField] Target _targetPrefab;
    [SerializeField] Target _targetStrongPrefab;
    [SerializeField] Target _targetFastPrefab;
    [SerializeField] Vector2 _yRange;

    [SerializeField] int _numberToFire;

    [SerializeField] Vector2 _respawnTime;
    [SerializeField] [Range(0, 1)] float _strongSpawnChance;
    [SerializeField] [Range(0, 1)] float _fastSpawnChance;

    float _screenHalfWidth;
    float _timeUntilSpawn;
    int _points;

    void Start()
    {
        _screenHalfWidth = new Vector2(
            Camera.main.aspect * Camera.main.orthographicSize,
            Camera.main.orthographicSize
        ).x;

        _points = 0;
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

        var val = Random.value;
        var isStrong = val < _strongSpawnChance;
        var isFast = val > _strongSpawnChance && val < _fastSpawnChance + _strongSpawnChance;
        var prefab = isStrong ? _targetStrongPrefab : isFast ? _targetFastPrefab : _targetPrefab;

        var target = Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
        target.OnCreated(OnFired, direction);

        _timeUntilSpawn = Random.Range(_respawnTime.x, _respawnTime.y);
    }

    public override void Begin()
    {
        _player.EnableControls();
    }

    public override void End()
    {
        _player.DisableControls();

        foreach (var obj in FindObjectsOfType<Target>())
        {
            Utilities.DestroyGameObject(obj.gameObject);
        }

        foreach (var obj in FindObjectsOfType<Fireball>())
        {
            Utilities.DestroyGameObject(obj.gameObject);
        }

        foreach (var obj in FindObjectsOfType<Explosion>())
        {
            Utilities.DestroyGameObject(obj.gameObject);
        }
    }

    void OnFired(int points)
    {
        _points += points;
        UpdateFiredCounter();

        if (_points >= _numberToFire)
            isDone = true;
    }

    void UpdateFiredCounter()
    {
        _firedCounter.text = $"{_points}/{_numberToFire}";
    }
}