using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapSaveLoad : MonoBehaviour
{
    [SerializeField] private EntityDataDB database;

    private string SavePath => Path.Combine(Application.persistentDataPath, "mapsave.json");

    public MapData CreateMapData(IEnumerable<Building> buildings, IEnumerable<Unit> units)
    {
        MapData mapData = new MapData();

        foreach (var building in buildings)
        {
            var data = building.GetData();
            Vector2Int cell = building.GetCellPosition();

            var record = new MapBuildingRecord
            {
                id = data.Id,
                cellX = cell.x,
                cellY = cell.y,
                teamId = (int)building.GetTeam(),
            };

            if (building is IUnitGenerator generator)
            {
                Vector3 rally = generator.GetUnitRallyPoint();
                record.hasRallyPoint = true;
                record.rallyX = rally.x;
                record.rallyY = rally.y;
                record.rallyZ = rally.z;
            }

            mapData.buildings.Add(record);
        }

        if (units == null) return mapData;
        foreach (var unit in units)
        {
            var data = unit.GetData();

            mapData.units.Add(new MapUnitRecord
            {
                id = data.Id,
                teamId = (int)unit.GetTeam(),
            });
        }

        return mapData;
    }

    public void SaveMapData(MapData mapData)
    {
        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Save Completed: {SavePath}");
    }

    public MapData LoadMapDataFromFile()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No files found");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        MapData mapData = JsonUtility.FromJson<MapData>(json);
        return mapData;
    }
}