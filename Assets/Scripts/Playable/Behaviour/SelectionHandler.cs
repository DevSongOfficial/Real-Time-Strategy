using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler
{
    private LayerMask layerMask;
    private Camera camera;
    private List<ISelectable> selectedUnits;

    public SelectionHandler(List<ISelectable> selectedUnits, Camera camera, LayerMask layerMask)
    {
        this.selectedUnits = selectedUnits;
        this.camera = camera;
        this.layerMask = layerMask;
    }

    public void HandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Allows mutil-selection.
        if (!Input.GetKey(KeyCode.LeftShift))
            DeselectAllUnits();

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if (hit.collider.TryGetComponent(out ISelectable unit))
        {
            SelectUnit(unit);
        }
    }

    private void SelectUnit(ISelectable unit)
    {
        if (!selectedUnits.Contains(unit))
            selectedUnits.Add(unit);

        unit.OnSelected();
    }

    public void SelectUnits(IEnumerable<ISelectable> units)
    {
        foreach (var unit in units)
            SelectUnit(unit);

    }

    private void DeselectUnit(ISelectable unit)
    {
        if (selectedUnits.Contains(unit))
            selectedUnits.Remove(unit);

        unit.OnDeselected();
    }

    private void DeselectAllUnits()
    {
        for (int i = selectedUnits.Count - 1; i >= 0; i--)
            DeselectUnit(selectedUnits[i]);
    }
}
