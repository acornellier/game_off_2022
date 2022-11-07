using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Renders a given inventory
/// /// </summary>
[RequireComponent(typeof(RectTransform))]
public class InventoryRenderer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The size of the cells building up the inventory")]
    Vector2Int _cellSize = new(32, 32);

    [SerializeField]
    [Tooltip("The sprite to use for empty cells")]
    Sprite _cellSpriteEmpty;

    [SerializeField]
    [Tooltip("The sprite to use for selected cells")]
    Sprite _cellSpriteSelected;

    [SerializeField]
    [Tooltip("The sprite to use for blocked cells")]
    Sprite _cellSpriteBlocked;

    internal IInventoryManager inventory;
    bool _haveListeners;
    Pool<Image> _imagePool;
    Image[] _grids;
    readonly Dictionary<IInventoryItem, Image> _items = new();

    /*
     * Setup
     */
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Create the image container
        imageContainer = new GameObject("Image Pool").AddComponent<RectTransform>();
        imageContainer.transform.SetParent(transform);
        imageContainer.anchorMin = new Vector2(0, 1);
        imageContainer.anchorMax = new Vector2(0, 1);
        imageContainer.pivot = new Vector2(0, 1);
        imageContainer.anchoredPosition = Vector3.zero;
        imageContainer.transform.localScale = Vector3.one;

        // Create pool of images
        _imagePool = new Pool<Image>(
            delegate
            {
                var image = new GameObject("Image").AddComponent<Image>();
                image.transform.SetParent(imageContainer);
                image.transform.localScale = Vector3.one;
                image.rectTransform.anchorMin = new Vector2(0, 1);
                image.rectTransform.anchorMax = new Vector2(0, 1);
                image.rectTransform.pivot = new Vector2(0, 1);
                return image;
            }
        );
    }

    /// <summary>
    /// Set what inventory to use when rendering
    /// </summary>
    public void SetInventory(IInventoryManager inventoryManager)
    {
        OnDisable();

        inventory = inventoryManager ??
                    throw new ArgumentNullException(nameof(inventoryManager));

        if (gameObject.activeInHierarchy && enabled)
            OnEnable();
    }

    /// <summary>
    /// Returns the RectTransform for this renderer
    /// </summary>
    RectTransform rectTransform { get; set; }

    /// <summary>
    /// Returns the RectTransform for this renderer
    /// </summary>
    public RectTransform imageContainer { get; private set; }

    /// <summary>
    /// Returns the size of this inventory's cells
    /// </summary>
    public Vector2 cellSize => _cellSize;

    /* 
    Invoked when the inventory inventoryRenderer is enabled
    */
    void OnEnable()
    {
        if (inventory == null || _haveListeners)
            return;

        if (_cellSpriteEmpty == null)
            throw new NullReferenceException("Sprite for empty cell is null");
        if (_cellSpriteSelected == null)
            throw new NullReferenceException("Sprite for selected cells is null.");
        if (_cellSpriteBlocked == null)
            throw new NullReferenceException("Sprite for blocked cells is null.");

        inventory.onRebuilt += ReRenderAllItems;
        inventory.onItemAdded += HandleItemAdded;
        inventory.onItemRemoved += HandleItemRemoved;
        inventory.onItemDropped += HandleItemRemoved;
        inventory.onResized += HandleResized;
        _haveListeners = true;

        // Render inventory
        ReRenderGrid();
        ReRenderAllItems();
    }

    /* 
    Invoked when the inventory inventoryRenderer is disabled
    */
    void OnDisable()
    {
        if (inventory == null || !_haveListeners)
            return;

        inventory.onRebuilt -= ReRenderAllItems;
        inventory.onItemAdded -= HandleItemAdded;
        inventory.onItemRemoved -= HandleItemRemoved;
        inventory.onItemDropped -= HandleItemRemoved;
        inventory.onResized -= HandleResized;
        _haveListeners = false;
    }

    /*
    Clears and renders the grid. This must be done whenever the size of the inventory changes
    */
    void ReRenderGrid()
    {
        // Clear the grid
        if (_grids != null)
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i].gameObject.SetActive(false);
                RecycleImage(_grids[i]);
                _grids[i].transform.SetSiblingIndex(i);
            }

        _grids = null;

        var containerSize = new Vector2(
            cellSize.x * inventory.width,
            cellSize.y * inventory.height
        );
        _grids = new Image[inventory.width * inventory.height];
        var c = 0;
        for (var y = 0; y < inventory.height; y++)
        {
            for (var x = 0; x < inventory.width; x++)
            {
                var grid = CreateImage(_cellSpriteEmpty, false);
                grid.gameObject.name = "Grid " + c;
                grid.type = Image.Type.Sliced;
                grid.rectTransform.localPosition =
                    new Vector3(cellSize.x * x, -cellSize.y * y, 0);
                _grids[c] = grid;
                c++;
            }
        }

        // Set the size of the main RectTransform
        // This is useful as it allowes custom graphical elements
        // suchs as a border to mimic the size of the inventory.
        rectTransform.sizeDelta = containerSize;
    }

    /*
    Clears and renders all items
    */
    void ReRenderAllItems()
    {
        // Clear all items
        foreach (var image in _items.Values)
        {
            image.gameObject.SetActive(false);
            RecycleImage(image);
        }

        _items.Clear();

        // Add all items
        foreach (var item in inventory.allItems)
        {
            HandleItemAdded(item);
        }
    }

    /*
    Handler for when inventory.OnItemAdded is invoked
    */
    void HandleItemAdded(IInventoryItem item)
    {
        var img = CreateImage(item.sprite, true);
        img.rectTransform.localPosition = GetItemOffset(item);
        img.rectTransform.localRotation = Quaternion.AngleAxis(item.rotation, new Vector3(0, 0, 1));
        _items.Add(item, img);
    }

    /*
    Handler for when inventory.OnItemRemoved is invoked
    */
    void HandleItemRemoved(IInventoryItem item)
    {
        if (_items.ContainsKey(item))
        {
            var image = _items[item];
            image.gameObject.SetActive(false);
            RecycleImage(image);
            _items.Remove(item);
        }
    }

    /*
    Handler for when inventory.OnResized is invoked
    */
    void HandleResized()
    {
        ReRenderGrid();
        ReRenderAllItems();
    }

    /*
     * Create an image with given sprite and settings
     */
    Image CreateImage(Sprite sprite, bool isItem)
    {
        var img = _imagePool.Take();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling();
        img.type = Image.Type.Simple;
        img.raycastTarget = !isItem;
        return img;
    }

    /*
     * Recycles given image 
     */
    void RecycleImage(Image image)
    {
        image.gameObject.name = "Image";
        image.gameObject.SetActive(false);
        _imagePool.Recycle(image);
    }

    /// <summary>
    /// Selects a given item in the inventory
    /// </summary>
    /// <param name="item">Item to select</param>
    /// <param name="blocked">Should the selection be rendered as blocked</param>
    /// <param name="color">The color of the selection</param>
    public void SelectItem(IInventoryItem item, bool blocked, Color color)
    {
        if (item == null)
            return;

        ClearSelection();

        for (var x = 0; x < item.width; x++)
        {
            for (var y = 0; y < item.height; y++)
            {
                if (!item.IsPartOfShape(new Vector2Int(x, y)))
                    continue;

                var p = item.position + new Vector2Int(x, y);
                if (p.x < 0 || p.x >= inventory.width || p.y < 0 || p.y >= inventory.height)
                    continue;

                var index = p.y * inventory.width + p.x;
                _grids[index].sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                _grids[index].color = color;
            }
        }
    }

    /// <summary>
    /// Clears all selections made in this inventory
    /// </summary>
    public void ClearSelection()
    {
        foreach (var t in _grids)
        {
            t.sprite = _cellSpriteEmpty;
            t.color = Color.white;
        }
    }

    /*
    Returns the appropriate offset of an item to make it fit nicely in the grid
    */
    internal Vector2 GetItemOffset(IInventoryItem item)
    {
        var position = item.position * cellSize;
        return new Vector2(position.x, -position.y);
    }
}