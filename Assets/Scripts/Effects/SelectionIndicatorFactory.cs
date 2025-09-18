using CustomResourceManagement;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class SelectionIndicatorFactory
{
    private Transform prefab;

    public SelectionIndicatorFactory()
    {
        prefab = ResourceLoader.GetResource<Transform>(Prefabs.Effect.SelectionIndicator);
    }

    public Transform Create()
    {
        var selectionIndicator = GameObject.Instantiate(prefab);
        return selectionIndicator;
    }
}
