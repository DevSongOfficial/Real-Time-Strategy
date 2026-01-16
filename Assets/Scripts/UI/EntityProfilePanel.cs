using System.Collections;
using UnityEngine;

public class EntityProfilePanel : MonoBehaviour
{
    [SerializeField] private BuildingProfileView buildingPV;
    [SerializeField] private UnitProfileView unitPV;

    private ISelectable currentEntity;
    private bool isSelected;

    public void RegisterEntity(ISelectable entity)
    {
        currentEntity = entity;
        isSelected = true;
    }

    public void UnregisterEntity()
    {
        currentEntity = null;
        isSelected = false;
    }

    private void LateUpdate()
    {
        if (!isSelected) return;

        if (currentEntity is Building building)
            buildingPV.Refresh(building);
        else if (currentEntity is Unit unit)
            unitPV.Refresh(unit);
    }
}