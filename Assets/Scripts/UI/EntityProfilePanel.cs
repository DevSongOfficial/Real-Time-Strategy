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


        buildingPV.DisableProgressInfoButtons();

        if (currentEntity is ResourceProvider resourceProvider)
            buildingPV.SetupUnitSlotSprites(resourceProvider.GetData().UnitSlotCount);
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
        {
            buildingPV.Refresh(building);
            
            if (currentEntity is ResourceProvider resourceProvider)
                RefreshUnitSlots(resourceProvider);
        }
        else if (currentEntity is Unit unit)
            unitPV.Refresh(unit);
    }

    private void RefreshUnitSlots(ResourceProvider resourceProvider)
    {
        buildingPV.ClearProgressInfoButtons();

        int index = 0;
        foreach (var unit in resourceProvider.GetRegisteredUnits())
        {
            var sprite = unit.GetData().ProfileSprite;
            buildingPV.FillUnitSlotSprites(sprite, index);
            index++;
        }
    }
}