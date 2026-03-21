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
    private readonly Func<UnitGenerator> getUnitGenerator;
    
    private readonly EntityProfilePanel profilePanel;
    private readonly RallyPointSetter spawnPositionSetter;

    public BuildingFactory(
        SelectionIndicatorFactory selectionIndicatorFactory = null,
        Func<UnitGenerator> getUnitGenerator = null,  
        EntityProfilePanel profilePanel= null, 
        RallyPointSetter spawnPositionSetter = null)
    {
        this.getUnitGenerator = getUnitGenerator;
        this.profilePanel = profilePanel;
        this.spawnPositionSetter = spawnPositionSetter;

        base.Setup(selectionIndicatorFactory);
    }

    public override Building Create(EntityData data, TeamContext teamContext)
    {
        var prefab = data.Prefab.GetComponent<Building>();
        var building = GameObject.Instantiate<Building>(prefab);

        // Set selection indicator.
        var selectionIndicator = CreateSelectionIndicator(building, data.RadiusOnTerrain);


        building.SetUp(data, selectionIndicator, profilePanel, teamContext);

        // For those spawning units e.g., barracks
        if (building is IUnitGenerator unitGenerator && getUnitGenerator != null)
        {
            unitGenerator?.SetUnitGenerator(getUnitGenerator.Invoke());
            unitGenerator?.SetSpawnPositionSetter(spawnPositionSetter);
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