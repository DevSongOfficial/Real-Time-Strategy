using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBuildingPreviewFactory
{
    Building CreateGhost(BuildingData data);
    void DestroyGhost(Building building);
}

public class BuildingFactory : PlayableAbsFactory<Building>, IBuildingPreviewFactory
{
    private SelectionIndicatorFactory selectionIndicatorFactory;
    private UnitGenerator unitGenerator;

    public BuildingFactory(UnitGenerator unitGenerator, SelectionIndicatorFactory selectionIndicatorFactory)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
        this.unitGenerator = unitGenerator;
    }

    public override Building Create(EntityData data, Team team)
    {
        var prefab = data.Prefab.GetComponent<Building>();
        var building = GameObject.Instantiate<Building>(prefab);

        // TODO: Seperate codes?
        // Create Selection Indicator. 
        var selectionIndicator = selectionIndicatorFactory.Create();
        selectionIndicator.parent = building.transform;
        selectionIndicator.localPosition = Vector3.zero.WithY(-building.PositionDeltaY);
        selectionIndicator.GetChild(0).localScale = (Vector3.one * data.RadiusOnTerrain).WithZ(1);
        selectionIndicator.gameObject.SetActive(false);

        building.SetUp(data, selectionIndicator.gameObject, Player.Team);

        // For those spawning units e.g., barracks
        if (building is IUnitGenerator unitGenerator)
            unitGenerator.Setup(this.unitGenerator);

        return building;
    }

    public Building CreateGhost(BuildingData data)
    {
        var buildingData = data.Prefab.GetComponent<Building>();
        var building = GameObject.Instantiate<Building>(buildingData);
        building.MakeRenderOnly();

        return building;
    }

    public void DestroyGhost(Building building)
    {
        GameObject.Destroy(building.gameObject);
    }
}