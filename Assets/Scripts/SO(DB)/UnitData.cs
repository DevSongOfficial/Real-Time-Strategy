using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : EntityData
{
    [field: SerializeField] public float TrainingTime { get; set; } = 4;
}
