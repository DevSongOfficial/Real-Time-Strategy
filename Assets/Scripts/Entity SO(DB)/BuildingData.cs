using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/Building Data")]
public class BuildingData : EntityData
{
    [field: SerializeField] public Vector2Int CellSize { get; private set; } // X, Z

    [field: Header("Construction")]
    [field: SerializeField] public float ConsructionTime { get; private set; } = 4f;
    [field: SerializeField] public int GoldRequired { get; private set; } = 0;
    [field: SerializeField] public int WoodRequired { get; private set; } = 0;

    [field: Header("Demolition")]
    [field: SerializeField] public float DemolitionTime { get; private set; } = 4f;
    [field: SerializeField] public int GoldRefund { get; private set; } = 0;
    [field: SerializeField] public int WoodRefund { get; private set; } = 0;
}
