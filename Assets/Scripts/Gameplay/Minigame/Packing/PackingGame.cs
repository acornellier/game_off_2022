using System;
using System.Linq;
using UnityEngine;

public class PackingGame : Minigame
{
    [SerializeField] Vector2Int _sourceDim = new(6, 6);
    [SerializeField] Vector2Int _destDim = new(4, 4);

    [SerializeField] InventoryRenderer _sourceRenderer;
    [SerializeField] InventoryRenderer _destRenderer;

    [SerializeField] Item[] _items;

    [ReadOnly] [SerializeField] int _destSpace;
    [ReadOnly] [SerializeField] int _itemsSpace;

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

        isDone = true;
    }

    void OnValidate()
    {
        _destSpace = _destDim.x * _destDim.y;
        _itemsSpace = _items.Aggregate(0, (cur, item) => cur + item.squaresTaken);
    }

    static void MonitorManager(IInventoryManager manager, string name)
    {
        manager.onItemAddedFailed += item => d.log(name, "add failed", item);
        manager.onItemDroppedFailed += item => d.log(name, "drop failed", item);
    }

    public override void Begin()
    {
        _sourceRenderer.draggingEnabled = true;
    }

    public override void End()
    {
        _sourceRenderer.draggingEnabled = false;
    }
}