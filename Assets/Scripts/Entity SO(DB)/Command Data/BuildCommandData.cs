
using UnityEngine;

[CreateAssetMenu(fileName = "Build Command", menuName = "Scriptable Objects/Command/Build")]
public class BuildCommandData : CommandData
{
    public override CommandType Type => CommandType.Build;

    // Building to generate
    [field: SerializeField] public BuildingData BuildingData { get; private set; }
    [field: SerializeField] public float GenerationTime { get; private set; }
}