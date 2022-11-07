using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectList", menuName = "Item/ItemObjectList", order = 0)]
public class ItemObjectList : ScriptableObject
{
    public List<Item> items;

    [ContextMenu("Validate")]
    void OnValidate()
    {
        var seenItems = new HashSet<string>();
        foreach (var item in items)
        {
            if (item.sprite == null)
                Debug.LogError($"ItemObject is missing sprite: {item.name}");

            if (seenItems.Contains(item.name))
                Debug.LogError($"Duplicate item in item list {name}: {item.name}");

            seenItems.Add(item.name);
        }
    }

    public Item FindByName(string itemName)
    {
        return items.Find(itemObject => itemObject.name == itemName);
    }
}