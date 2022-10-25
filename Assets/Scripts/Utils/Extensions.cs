using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T GetComponentInDirectChildren<T>(this GameObject gameObject)
        where T : Component
    {
        var length = gameObject.transform.childCount;
        for (var i = 0; i < length; i++)
        {
            var comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) return comp;
        }

        return null;
    }

    public static List<T> GetComponentsInDirectChildren<T>(this GameObject gameObject)
        where T : Component
    {
        var length = gameObject.transform.childCount;
        var components = new List<T>(length);
        for (var i = 0; i < length; i++)
        {
            var comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }

        return components;
    }

    public static bool IsLayerInMask(this GameObject gameObject, LayerMask mask)
    {
        return mask == (mask | (1 << gameObject.layer));
    }

    public static T GetComponentInParents<T>(this GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }

        return comp;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return !source.Any();
    }

    public static Vector3 ToVector3(this Vector2Int v)
    {
        return new Vector3(v.x, v.y);
    }
}