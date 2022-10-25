using System.Linq;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public static class d
{
    // ReSharper disable once InconsistentNaming
    public static void log(params object[] v)
    {
        Debug.Log(string.Join(", ", v.Select(s => s.ToString())));
    }
}