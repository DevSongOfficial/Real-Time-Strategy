using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/Building Data")]
public class BuildingData : EntityData
{
    [field: SerializeField] public Vector2Int CellSize { get; private set; } // X, Z

    [field: Header("Construction")]
    [field: SerializeField] public float ConsructionTime { get; private set; } = 4f;
    [field: SerializeField] public int RequiredGold { get; private set; } = 0;
    [field: SerializeField] public int RequiredWood { get; private set; } = 0;
}
