using UnityEngine;

[CreateAssetMenu(fileName = "Unit Train", menuName = "Scriptable Objects/UnitTrainCommandData")]
public class UnitTrainCommandData : CommandData
{
    public override CommandType Type => CommandType.UnitTrain;

    [field: SerializeField] public UnitData UnitData { get; private set; }
    [field: SerializeField] public float GenerationTime { get; private set; }
}