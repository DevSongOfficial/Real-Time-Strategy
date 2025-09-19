using UnityEngine;

public interface IBuildingPreviewFactory
{
    Building CreateGhost(BuildingData data);
    void DestroyGhost(Building building);
}

public class BuildingFactory : PlayableAbsFactory<Building>, IBuildingPreviewFactory
{
    private SelectionIndicatorFactory selectionIndicatorFactory;

    public BuildingFactory(SelectionIndicatorFactory selectionIndicatorFactory)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
    }

    public override Building Create(EntityData data)
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

        building.SetUp(data, selectionIndicator.gameObject);

        return building;
    }

    public Building CreateGhost(BuildingData data)
    {
        var building = data.Prefab.GetComponent<Building>();
        // disable components of data, and then,
        return GameObject.Instantiate<Building>(building);
    }

    public void DestroyGhost(Building building)
    {
        GameObject.Destroy(building.gameObject);
    }
}
