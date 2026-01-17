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

        var isBuilding = currentEntity is Building;
        buildingPV.gameObject.SetActive(isBuilding);
        unitPV.gameObject.SetActive(!isBuilding);
    }

    public void UnregisterEntity()
    {
        currentEntity = null;
        isSelected = false;

        buildingPV.gameObject.SetActive(false);
        unitPV.gameObject.SetActive(false);
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