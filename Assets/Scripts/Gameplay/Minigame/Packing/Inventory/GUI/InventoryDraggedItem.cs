using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for keeping track of dragged items
/// </summary>
public class InventoryDraggedItem
{
    /// <summary>
    /// Returns the InventoryController this item originated from
    /// </summary>
    readonly InventoryController _originalController;

    /// <summary>
    /// Returns the point inside the inventory from which this item originated from
    /// </summary>
    readonly Vector2Int _originPoint;

    /// <summary>
    /// Returns the item-instance that is being dragged
    /// </summary>
    readonly IInventoryItem _item;

    /// <summary>
    /// Gets or sets the InventoryController currently in control of this item
    /// </summary>
    public InventoryController currentController;

    public bool dropFailed;

    readonly Canvas _canvas;
    readonly RectTransform _canvasRect;
    readonly Image _image;
    Vector2 _offset;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="canvas">The canvas</param>
    /// <param name="originalController">The InventoryController this item originated from</param>
    /// <param name="originPoint">The point inside the inventory from which this item originated from</param>
    /// <param name="item">The item-instance that is being dragged</param>
    /// <param name="offset">The starting offset of this item</param>
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public InventoryDraggedItem(
        Canvas canvas,
        InventoryController originalController,
        Vector2Int originPoint,
        IInventoryItem item,
        Vector2 offset)
    {
        _originalController = originalController;
        currentController = _originalController;
        _originPoint = originPoint;
        _item = item;

        _canvas = canvas;
        _canvasRect = canvas.transform as RectTransform;

        // _offset = offset;
        _offset = default;

        // Create an image representing the dragged item
        _image = new GameObject("DraggedItem").AddComponent<Image>();
        _image.raycastTarget = false;
        _image.transform.SetParent(_canvas.transform);
        _image.transform.SetAsLastSibling();
        _image.sprite = item.sprite;
        _image.SetNativeSize();
        _image.rectTransform.localRotation =
            Quaternion.AngleAxis(item.rotation, new Vector3(0, 0, 1));
        _image.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Gets or sets the position of this dragged item
    /// </summary>
    public Vector2 position
    {
        set
        {
            if (!_canvas) return;

            // Movethe image
            var camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                value + _offset,
                camera,
                out var newValue
            );
            _image.rectTransform.localPosition = newValue;

            // Make selections
            if (currentController != null)
            {
                var draggedOffset = GetDraggedItemOffset(
                    currentController.inventoryRenderer,
                    _item
                );

                _item.position = currentController.ScreenToGrid(
                    value +
                    _offset +
                    draggedOffset
                );

                var canAdd = currentController.inventory.CanAddAt(_item, _item.position) ||
                             CanSwapAt(_item.position);
                currentController.inventoryRenderer.SelectItem(_item, !canAdd, Color.white);
            }

            // Slowly animate the item towards the center of the mouse pointer
            _offset = Vector2.Lerp(_offset, Vector2.zero, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// Drop this item at the given position
    /// </summary>
    public bool Drop(Vector2 pos)
    {
        if (currentController == null)
        {
            dropFailed = true;
            return false;
        }

        var grid = currentController.ScreenToGrid(
            pos + _offset + GetDraggedItemOffset(currentController.inventoryRenderer, _item)
        );

        // Try to add new item
        if (currentController.inventory.CanAddAt(_item, grid))
        {
            // Place the item in a new location
            currentController.inventory.TryAddAt(_item, grid);
        }
        // Adding did not work, try to swap
        else if (CanSwapAt(grid))
        {
            var otherItem = currentController.inventory.allItems[0];
            currentController.inventory.TryRemove(otherItem);
            _originalController.inventory.TryAdd(otherItem);
            currentController.inventory.TryAdd(_item);
        }
        else if (!_originalController.inventory.CanAddAt(_item, _originPoint))
            // Could not return
        {
            dropFailed = true;
            return false;
        }
        // Return
        else
        {
            // Return the item to its previous location
            _originalController.inventory.TryAddAt(_item, _originPoint);
        }

        currentController.inventoryRenderer.ClearSelection();

        dropFailed = false;

        // Destroy the image representing the item
        Object.Destroy(_image.gameObject);

        return true;
    }

    public void RotateCw()
    {
        _item.RotateCw();
        _image.rectTransform.localRotation =
            Quaternion.AngleAxis(_item.rotation, new Vector3(0, 0, 1));
    }

    /*
     * Returns the offset between dragged item and the grid 
     */
    Vector2 GetDraggedItemOffset(InventoryRenderer renderer, IInventoryItem item)
    {
        var scale = new Vector2(
            Screen.width / _canvasRect.sizeDelta.x,
            Screen.height / _canvasRect.sizeDelta.y
        );
        var gx = -(item.width * renderer.cellSize.x / 2f) + renderer.cellSize.x / 2;
        var gy = item.height * renderer.cellSize.y / 2f - renderer.cellSize.y / 2;
        return new Vector2(gx, gy) * scale;
    }

    /* 
     * Returns true if its possible to swap
     */
    bool CanSwapAt(Vector2Int point)
    {
        if (!currentController.inventory.CanSwapAt(_item, point)) return false;

        var otherItem = currentController.inventory.GetAtPoint(point);
        if (otherItem == null) return false;

        return _originalController.inventory.CanAddAt(otherItem, _originPoint) &&
               currentController.inventory.CanRemove(otherItem);
    }
}