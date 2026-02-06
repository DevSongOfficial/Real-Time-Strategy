using System;
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
    private event Func<UnitGenerator> getUnitGenerator;
    private EntityProfilePanel profilePanel;
    private RallyPointSetter spawnPositionSetter;

    public BuildingFactory(Func<UnitGenerator> getUnitGenerator, ISelectionEvent selectionHandler, 
        SelectionIndicatorFactory selectionIndicatorFactory, EntityProfilePanel profilePanel, RallyPointSetter spawnPositionSetter)
    {
        this.getUnitGenerator = getUnitGenerator;
        this.profilePanel = profilePanel;
        this.spawnPositionSetter = spawnPositionSetter;

        base.Setup(selectionHandler, selectionIndicatorFactory);
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

        building.SetUp(data, selectionIndicator.gameObject, profilePanel, team);

        // For those spawning units e.g., barracks
        if (building is IUnitGenerator unitGenerator)
        {
            unitGenerator.SetUnitGenerator(getUnitGenerator.Invoke());
            unitGenerator.SetSpawnPositionSetter(spawnPositionSetter);
        }

        return building;
    }

    public Building CreateGhost(BuildingData data)
    {
        var buildingData = data.Prefab.GetComponent<Building>();
        var building = GameObject.Instantiate<Building>(buildingData);
        building.SetToPreview();

        return building;
    }

    public void DestroyGhost(Building building)
    {
        GameObject.Destroy(building.gameObject);
    }
}