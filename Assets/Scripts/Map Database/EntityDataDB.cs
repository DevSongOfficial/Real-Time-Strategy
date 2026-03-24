using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EntityDataDB : MonoBehaviour
{
    [SerializeField] private List<BuildingData> buildingDatas;
    [SerializeField] private List<UnitData> unitDatas;

    private Dictionary<string, BuildingData> lookupBuilding;
    private Dictionary<string, UnitData> lookupUnit;


    private void Awake()
    {
        lookupBuilding  = buildingDatas.ToDictionary(x => x.Id, x => x);
        lookupUnit      = unitDatas.ToDictionary(x => x.Id, x => x);
    }

    public BuildingData GetBuildinigData(string id)
    {
        if (lookupBuilding.TryGetValue(id, out var data))
            return data;

        Debug.LogError($"BuildingData not found. id = {id}");
        return null;
    }

    public UnitData GetUnitData(string id)
    {
        if (lookupUnit.TryGetValue(id, out var data))
            return data;

        Debug.LogError($"BuildingData not found. id = {id}");
        return null;
    }

    public IEnumerable<BuildingData> GetBuildingDatas()
    {
        foreach (var data in buildingDatas)
            yield return data;
    }
}