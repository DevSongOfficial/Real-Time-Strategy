using System.Collections.Generic;
using UnityEngine;

public class MovementHandler
{
    private LayerMask layerMask;
    private Camera camera;
    private IEnumerable<IMovable> movableUnits;

    public MovementHandler(IEnumerable<IMovable> movableUnits, Camera camera, LayerMask layerMask)
    {
        this.movableUnits = movableUnits;
        this.camera = camera;
        this.layerMask = layerMask;
    }

    public void HandleUnitMovement()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        foreach (var unit in movableUnits)
            unit.MoveTo(hit.point);
    }
}