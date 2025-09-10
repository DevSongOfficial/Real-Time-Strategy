using UnityEditor;
using UnityEngine;

public interface IBuildingPreviewFactory
{
    Building CreateGhost(BuildingData data);
    void DestroyGhost(Building building);
}

public class BuildingFactory : PlayableAbsFactory<Building>, IBuildingPreviewFactory
{
    public override Building Create(EntityData data)
    {
        var building = data.Prefab.GetComponent<Building>();
        return GameObject.Instantiate<Building>(building);
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
