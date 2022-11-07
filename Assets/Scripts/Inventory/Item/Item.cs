using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 0)]
public class Item : ScriptableObject, IInventoryItem
{
    [SerializeField] Sprite _sprite;
    [SerializeField] InventoryShape _shape;

    public string Name => name;
    public Sprite sprite => _sprite;
    public Vector2Int position { get; set; } = Vector2Int.zero;
    public int width => _shape.width;
    public int height => _shape.height;
    public int rotation { get; private set; }

    public bool canDrop => true;

    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape(localPosition);
    }

    public Item CreateInstance()
    {
        var clone = Instantiate(this);
        clone.name = clone.name[..^7]; // Remove (Clone) from name
        return clone;
    }

    public void Rotate()
    {
        rotation += 90;
        _shape.Rotate();
    }
}