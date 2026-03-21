using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : EntityData
{
    [field: Space]
    [field: SerializeField] public int CapacityCost { get; private set; } = 0;

    [field: SerializeField] public float TrainingTime { get; set; } = 4;
}
