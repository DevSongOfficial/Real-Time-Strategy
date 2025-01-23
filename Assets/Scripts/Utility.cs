using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    // OfType<T>() -> Lazy Evaluation.
    public static IEnumerable<TTarget> FilterByType<TSource, TTarget>(this IEnumerable<TSource> sourceList)
    {
        return sourceList.OfType<TTarget>();
    }
}
