using System.Collections;
using UnityEngine;

public class EntityProfilePanel : MonoBehaviour
{
    [SerializeField] private BuildingProfileView buildingPV;
    [SerializeField] private UnitProfileView unitPV;

    public ISelectable CurrentEntity { get; private set; }
    private bool isSelected;

    public void RegisterEntity(ISelectable entity)
    {
        CurrentEntity = entity;
        isSelected = true;

        var isBuilding = CurrentEntity is Building;
        buildingPV.gameObject.SetActive(isBuilding);
        unitPV.gameObject.SetActive(!isBuilding);


        buildingPV.DisableProgressInfoButtons();

        if (CurrentEntity is ResourceProvider resourceProvider)
            buildingPV.SetupUnitSlotSprites(resourceProvider.GetData().UnitSlotCount);
    }

    public void UnregisterEntity()
    {
        CurrentEntity = null;
        isSelected = false;

        buildingPV.gameObject.SetActive(false);
        unitPV.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!isSelected) return;

        if (CurrentEntity is Building building)
        {
            buildingPV.Refresh(building);
            
            if (CurrentEntity is ResourceProvider resourceProvider)
                RefreshUnitSlots(resourceProvider);
        }
        else if (CurrentEntity is Unit unit)
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