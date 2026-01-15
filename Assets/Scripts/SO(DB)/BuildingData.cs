using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/Building Data")]
public class BuildingData : EntityData
{
    [field: SerializeField] public Vector2Int CellSize { get; private set; } // X, Z
    [field: SerializeField] public float ConsructionTime { get; private set; } = 4f;
}
