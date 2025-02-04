using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Layer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    Ground = 6,
    Selectable = 11,
}

public static class Utility
{
    public static bool CompareLayer(this Collider collider, Layer layer)
    {
        return collider.gameObject.layer == (int)layer;
    }

    public static LayerMask ToLayerMask(this Layer layer)
    {
        return 1 << (int)layer;
    }

    public static LayerMask GetLayerMask(params Layer[] layers)
    {
        int mask = 0;
        foreach (var layer in layers)
        {
            mask |= 1 << (int)layer;
        }
        return mask;
    }

    // OfType<T>() -> Lazy Evaluation.
    public static IEnumerable<TTarget> FilterByType<TSource, TTarget>(this IEnumerable<TSource> sourceList)
    {
        return sourceList.OfType<TTarget>();
    }


    public static Vector3 WithX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 WithY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 WithZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3Int WithX(this Vector3Int vector, int x)
    {
        return new Vector3Int(x, vector.y, vector.z);
    }

    public static Vector3Int WithY(this Vector3Int vector, int y)
    {
        return new Vector3Int(vector.x, y, vector.z);
    }

    public static Vector3Int WithZ(this Vector3Int vector, int z)
    {
        return new Vector3Int(vector.x, vector.y, z);
    }
}
