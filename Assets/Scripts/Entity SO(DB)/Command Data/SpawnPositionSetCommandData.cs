using UnityEngine;

[CreateAssetMenu(fileName = "SetSpawnpoint", menuName = "Scriptable Objects/Command/Set Spawn Point")]
public class SpawnPositionSetCommandData : CommandData
{
    public override CommandType Type => CommandType.Utility;
}
