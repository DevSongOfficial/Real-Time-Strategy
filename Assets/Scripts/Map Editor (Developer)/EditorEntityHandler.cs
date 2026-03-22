using System.Collections.Generic;
using UnityEngine;

public class EditorEntityHandler
{
    private Camera camera;
    public EditorEntityHandler(Camera camera)
    {
        this.camera = camera;
    }

    public Playable SelectEntity(Vector2 screenPosition)
    {
        var ray = camera.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Selectable.ToLayerMask())
         || !hit.transform.parent.TryGetComponent(out Playable entity))
        {
            return null;
        }
    
        return entity;
    }

    public void Remove(Playable entity)
    {
        GameObject.Destroy(entity.gameObject);
    }
}