using System;
using System.Collections.Generic;
using UnityEngine;

public struct Target
{
    // For preventing null check.
    public bool IsGround { get; private set; } // true if it's terrain.
    public ITarget Entity => entity;

    private ITarget entity;
    private Vector3 hitPoint; // valid only it's ground.

    public Target(ITarget target)
    {
        entity = target;
        hitPoint = Vector3.zero;
        IsGround = false;
    }

    
    public Target(Vector3 position)
    {
        IsGround = true;
        entity = null;
        hitPoint = position;
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
    public void SelectTarget(Vector2 screenPosition)
    {
        var ray = camera.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Utility.GetLayerMask(Layer.Ground, Layer.Selectable) /*, TODO: LayerMask: Ground || Entity */))
            return;

        var target = new Target(hit);

        if (target.IsGround)
        {
            int count = 0;
            foreach(var unit in selectedEntities)
                if (unit is ITargetor) 
                    count++;

            var slots = BuildGridSlots(target.GetPosition(), count, 1f);
            for(int i = 0; i < selectedEntities.Count; i++)
                if (selectedEntities[i] is ITargetor targetor)
                    targetor.SetTarget(new Target(slots[i]));
        }
        else
        {
            foreach (var unit in selectedEntities)
                if (unit is ITargetor targetor)
                    targetor.SetTarget(target);
        }

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

    private List<Vector3> BuildGridSlots(Vector3 target, int count, float spacing)
    {
        // Slots to return
        var slots = new List<Vector3>(count);

        // 정사각형 모양에 가까운 Grid 생성
        // e.g., if count == 5 -> (3,2) 6 slots available
        int cols = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt(count / (float)cols);

        // Grid의 좌표 설정하기 
        float colCenter = (cols - 1) / 2f;
        float rowCenter = 0f;

        for (int i = 0; i < count; i++)
        {
            // Grid 채우기
            int row = i / cols; 
            int col = i % cols;

            // Grid 기반으로 실제 좌표 계산
            float x = (col - colCenter) * spacing;
            float z = -(row - rowCenter) * spacing;

            Vector3 position = target + Vector3.right * x + Vector3.forward * z;
            slots.Add(position);
        }

        return slots;
    }
}
