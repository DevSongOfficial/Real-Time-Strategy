using UnityEngine;

[CreateAssetMenu(fileName = "BarracksData", menuName = "Scriptable Objects/BarracksData")]
public class BarracksData : BuildingData
{
    [field: SerializeField] public EntityData[] SpawnableUnits { get; private set; }
}
