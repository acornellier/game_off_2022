using System.Collections;
using TMPro;
using UnityEngine;

public class PackingGame : Minigame
{
    [SerializeField] Vector2Int _sourceDim = new(6, 6);
    [SerializeField] Vector2Int _destDim = new(4, 4);

    [SerializeField] InventoryRenderer _sourceRenderer;
    [SerializeField] InventoryRenderer _destRenderer;

    [SerializeField] Item[] _items;

    [SerializeField] TMP_Text _victoryText;

    InventoryManager _source;
    InventoryManager _dest;

    bool _victoryStarted;

    void Awake()
    {
        var sourceProvider = new InventoryProvider();
        _source = new InventoryManager(sourceProvider, _sourceDim.x, _sourceDim.y);
        MonitorManager(_source, "source");

        var destProvider = new InventoryProvider();
        _dest = new InventoryManager(destProvider, _destDim.x, _destDim.y);
        MonitorManager(_dest, "dest");
    }

    void Start()
    {
        _sourceRenderer.SetInventory(_source);
        _destRenderer.SetInventory(_dest);

        foreach (var item in _items)
        {
            if (!_source.TryAdd(item.CreateInstance()))
                d.log("Failed to add to source", item);
        }
    }

    void Update()
    {
        if (_dest.allItems.Length < _items.Length)
            return;

        if (_victoryStarted)
            return;

        isDone = true;
        _victoryStarted = true;
        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        _victoryText.gameObject.SetActive(true);

        var t = 0f;
        const float duration = 3f;
        while (t < duration)
        {
            t += Time.deltaTime;
            var frac = t / duration;
            _victoryText.transform.localRotation = Quaternion.Euler(0, 0, frac * 5 * 360);
            _victoryText.fontSize = Mathf.Lerp(0, 60, frac);
            yield return null;
        }
    }

    static void MonitorManager(IInventoryManager manager, string name)
    {
        manager.onItemAdded += item => d.log(name, "added", item);
        manager.onItemDropped += item => d.log(name, "dropped", item);
        manager.onItemRemoved += item => d.log(name, "removed", item);
        manager.onItemAddedFailed += item => d.log(name, "add failed", item);
        manager.onItemDroppedFailed += item => d.log(name, "drop failed", item);
    }

    public override void Begin()
    {
    }

    public override void End()
    {
    }
}