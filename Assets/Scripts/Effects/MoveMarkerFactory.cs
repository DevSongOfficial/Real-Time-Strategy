using CustomResourceManagement;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

// TODO: Make markers fade away when released
public class MoveMarkerFactory
{
    private Transform prefab;
    private List<Transform> markerPool;

    public MoveMarkerFactory()
    {
        markerPool = new List<Transform>();
        prefab = ResourceLoader.GetResource<Transform>(Prefabs.Effect.MoveMarker);
    }

    public Transform Spawn(Vector3 position)
    {
        var newMarker = Get();
        newMarker.position = position.WithY(0.01f);

        foreach (Transform marker in markerPool) 
            marker.gameObject.SetActive(false);
        
        newMarker.gameObject.SetActive(true);
        return newMarker;
    }

    private Transform Get()
    {
        foreach(Transform marker in markerPool)
        {
            if(!marker.gameObject.activeSelf)
            {
                marker.gameObject.SetActive(true);
                return marker;
            }
        }

        var moveMarker = GameObject.Instantiate(prefab);
        markerPool.Add(moveMarker);
        return moveMarker;
    }
}