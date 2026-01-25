using UnityEngine;

[CreateAssetMenu(fileName = "Unit Train", menuName = "Scriptable Objects/Command/Train Unit")]
public class UnitTrainCommandData : CommandData
{
    public override CommandType Type => CommandType.Train;

    [field: SerializeField] public UnitData UnitData { get; private set; }
    [field: SerializeField] public float GenerationTime { get; private set; }
}