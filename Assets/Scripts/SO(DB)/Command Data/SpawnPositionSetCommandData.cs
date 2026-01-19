using UnityEngine;

[CreateAssetMenu(fileName = "SetSpawnpoint", menuName = "Scriptable Objects/Spawn Point Set Command Data")]
public class SpawnPositionSetCommandData : CommandData
{
    public override CommandType Type => CommandType.Utility;
}
