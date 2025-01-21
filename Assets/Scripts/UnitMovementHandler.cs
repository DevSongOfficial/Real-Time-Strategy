using System.Collections.Generic;
using UnityEngine;

public class UnitMovementHandler
{
    private LayerMask layerMask;
    private Camera camera;
    private List<ISelectable> selectedUnits;

    public UnitMovementHandler(List<ISelectable> selectedUnits, Camera camera, LayerMask layerMask)
    {
        this.selectedUnits = selectedUnits;
        this.camera = camera;
        this.layerMask = layerMask;
    }

    public void HandleUnitMovement()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (selectedUnits.Count == 0) return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        foreach (var unit in selectedUnits)
            MoveUnit(unit, hit.point);

    }

    private void MoveUnit(ISelectable unit, Vector3 destination)
    {
        if (unit is IMovable movable)
        {
            movable.MoveTo(destination);
        }
    }
}