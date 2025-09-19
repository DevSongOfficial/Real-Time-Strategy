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
            entity = hit.collider.GetComponentInParent<ITarget>();
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
    private List<ISelectable> selectedEntities;

    public SelectionHandler(List<ISelectable> selectedEntities, Camera camera)
    {
        this.selectedEntities = selectedEntities;
        this.camera = camera;
    }
    
    // Select our units' target to attack or move towards. (Mouse 1)
    public void SelectTarget(Vector2 screenPos)
    {
        var ray = camera.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Utility.GetLayerMask(Layer.Ground, Layer.Selectable) /*, TODO: LayerMask: Ground || Entity */))
            return;

        var target = new Target(hit);
        foreach (var unit in selectedEntities)
            if (unit is ITargetor targetor) 
                targetor.SetTarget(target);
    }

    // Select an entity to control. (Mouse 0)
    public void SelectEntity(Vector2 screenPos, bool additive)
    {
        if (!additive) DeselectAllUnits();

        var ray = camera.ScreenPointToRay(screenPos);
        // TODO: Only allow selection of same team units.
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Selectable.ToLayerMask()))
            return;

        if (!hit.transform.parent.TryGetComponent(out ISelectable entity))
            return;

        SelectEntity(entity);
    }

    public void SelectEntities(IEnumerable<ISelectable> entities)
    {
        foreach (var entity in entities)
            SelectEntity(entity);
    }

    private void SelectEntity(ISelectable entity)
    {
        if (!selectedEntities.Contains(entity))
            selectedEntities.Add(entity);

        entity.OnSelected();
    }

    private void DeselectUnit(ISelectable unit)
    {
        if (selectedEntities.Contains(unit))
            selectedEntities.Remove(unit);

        unit.OnDeselected();
    }

    private void DeselectAllUnits()
    {
        for (int i = selectedEntities.Count - 1; i >= 0; i--)
            DeselectUnit(selectedEntities[i]);
    }
}
