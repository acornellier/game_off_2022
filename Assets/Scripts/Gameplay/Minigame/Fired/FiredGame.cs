using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FiredGame : Minigame
{
    [SerializeField] FiredPlayer _player;
    [SerializeField] TMP_Text _firedCounter;
    [SerializeField] Vector2 _yRange;

    [SerializeField] TargetConfig[] _targets;
    [SerializeField] int _numberToFire;
    [SerializeField] Vector2 _respawnTime;

    bool _running;
    WeightedDistribution _weightedDistribution;
    float _screenHalfWidth;
    float _timeUntilSpawn;
    int _points;

    void Start()
    {
        _screenHalfWidth = new Vector2(
            Camera.main.aspect * Camera.main.orthographicSize,
            Camera.main.orthographicSize
        ).x;


        _weightedDistribution = new WeightedDistribution(_targets.Select(t => t.weight));

        _points = 0;
        UpdateFiredCounter();
    }

    void Update()
    {
        if (!_running) return;

        _timeUntilSpawn -= Time.deltaTime;
        if (_timeUntilSpawn > 0)
            return;

        var isLeft = Random.value > 0.5f;
        var x = _screenHalfWidth * (isLeft ? -1 : 1);
        var y = Random.Range(_yRange.x, _yRange.y);
        var direction = isLeft ? Vector3.right : Vector3.left;

        var type = _weightedDistribution.PickRandom();
        var prefab = _targets[type].prefab;
        var target = Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
        target.OnCreated(OnFired, direction);

        _timeUntilSpawn = Random.Range(_respawnTime.x, _respawnTime.y);
    }

    public override void Begin()
    {
        _running = true;
        _player.EnableControls();
    }

    public override void End()
    {
        _running = false;
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

    [Serializable]
    public class TargetConfig
    {
        public Target prefab;
        public float weight;
    }
}