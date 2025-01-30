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
    Ground = 6
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
}
