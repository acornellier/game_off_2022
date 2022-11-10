using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for storing the shape of an inventory item
/// </summary>
[Serializable]
public class InventoryShape
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] bool[] _shape;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="width">The maximum width of the shape</param>
    /// <param name="height">The maximum height of the shape</param>
    public InventoryShape(int width, int height)
    {
        _width = width;
        _height = height;
        _shape = new bool[_width * _height];
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="shape">A custom shape</param>
    public InventoryShape(bool[,] shape)
    {
        Initialize(shape);
    }

    void Initialize(bool[,] shape)
    {
        _width = shape.GetLength(0);
        _height = shape.GetLength(1);
        _shape = new bool[_width * _height];
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                _shape[GetIndex(x, y)] = shape[x, y];
            }
        }
    }

    /// <summary>
    /// Returns the width of the shapes bounding box
    /// </summary>
    public int width => _width;

    /// <summary>
    /// Returns the height of the shapes bounding box
    /// </summary>
    public int height => _height;

    /// <summary>
    /// Returns true if given local point is part of this shape
    /// </summary>
    public bool IsPartOfShape(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 ||
            localPoint.y >= _height)
            return false; // outside of shape width/height

        var index = GetIndex(localPoint.x, localPoint.y);
        return _shape[index];
    }

    public void RotateCW()
    {
        var newCoords = new List<Vector2Int>();
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                if (_shape[GetIndex(x, y)])
                    newCoords.Add(new Vector2Int(-y, x));
            }
        }

        var minX = int.MaxValue;
        var minY = int.MaxValue;
        foreach (var coord in newCoords)
        {
            if (coord.x < minX) minX = coord.x;
            if (coord.y < minY) minY = coord.y;
        }

        for (var i = 0; i < newCoords.Count; ++i)
        {
            newCoords[i] -= new Vector2Int(minX, minY);
        }

        var newWidth = _height;
        var newHeight = _width;
        var newShape = new bool[newWidth, newHeight];
        for (var x = 0; x < newWidth; x++)
        {
            for (var y = 0; y < newHeight; y++)
            {
                newShape[x, y] = newCoords.Contains(new Vector2Int(x, y));
            }
        }

        Initialize(newShape);
    }

    /*
    Converts X & Y to an index to use with _shape
    */
    int GetIndex(int x, int y)
    {
        return x + _width * y;
    }
}