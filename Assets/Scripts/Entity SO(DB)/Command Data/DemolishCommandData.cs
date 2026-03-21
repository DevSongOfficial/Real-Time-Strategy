using UnityEngine;

[CreateAssetMenu(fileName = "Demolish", menuName = "Scriptable Objects/Command/Demolish")]
public class DemolishCommandData : CommandData
{
    public override CommandType Type => CommandType.Utility;
}
