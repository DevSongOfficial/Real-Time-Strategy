using System;
using System.Collections.Generic;

[Serializable]
public class MapData
{
    public List<MapBuildingRecord> buildings = new();
    public List<MapUnitRecord> units = new();
}

[Serializable]
public class MapBuildingRecord
{
    // Static data can be accessed through this id.
    public string id;

    // Dynamic data cannot be accessed via id so should be stored below.
    public int cellX;
    public int cellY;
    public int teamId;

    public bool hasRallyPoint;
    public float rallyX;
    public float rallyY;
    public float rallyZ;
}

[Serializable]
public class MapUnitRecord
{
    // Static data can be accessed through this id.
    public string id;

    // Dynamic data cannot be accessed via id so should be stored below.
    public int teamId;
}