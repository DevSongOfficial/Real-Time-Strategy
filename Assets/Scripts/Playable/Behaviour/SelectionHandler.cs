using System;
using System.Collections.Generic;
using UnityEngine;

public struct Target
{
    // For preventing null check.
    public bool IsGround { get; private set; }
    public ITarget Entity => entity;

    private ITarget entity;
    private Vector3 hitPoint;

    public Target(ITarget target)
    {
        entity = target;
        hitPoint = Vector3.zero;
        IsGround = false;
    }

    public Target(RaycastHit hit)
    {
        if (hit.collider.CompareLayer(Layer.Ground))
        {
            IsGround = true;
            entity = null;
            hitPoint = hit.point;
        }
        else
        {
            IsGround = false;
            entity = hit.collider.GetComponent<ITarget>();
            hitPoint = entity.GetPosition();
        }
    }

    public Vector3 GetPosition()
    {
        return IsGround ? hitPoint : entity.GetPosition();
    }
}
public class SelectionHandler
{
    private Camera camera;
    private List<ISelectable> selectedUnits;

    public SelectionHandler(List<ISelectable> selectedUnits, Camera camera)
    {
        this.selectedUnits = selectedUnits;
        this.camera = camera;
    }

    public void HandleTargetSelection()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity /*, LayerMask: Ground || Enemy Playble */)) return;

        var target = new Target(hit);
        foreach(var unit in selectedUnits)
        {
            if(unit is ITargetor targetor)
                targetor.SetTarget(target);
        }
    }

    public void HandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Allows mutil-selection.
        if (!Input.GetKey(KeyCode.LeftShift))
            DeselectAllUnits();

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        // TODO: Only allow selection of same team units.
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Selectable.ToLayerMask())) return;

        if (hit.collider.TryGetComponent(out ISelectable unit))
        {
            SelectUnit(unit);
        }
    }

    public void SelectUnits(IEnumerable<ISelectable> units)
    {
        foreach (var unit in units)
            SelectUnit(unit);

    }

    private void SelectUnit(ISelectable unit)
    {
        if (!selectedUnits.Contains(unit))
            selectedUnits.Add(unit);

        unit.OnSelected();
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
