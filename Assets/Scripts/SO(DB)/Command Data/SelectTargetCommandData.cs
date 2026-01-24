using UnityEngine;

[CreateAssetMenu(fileName = "SelectTarget", menuName = "Scriptable Objects/Select Target Command Data")]
public class SelectTargetCommandData : CommandData
{
    public override CommandType Type => CommandType.Utility;

}
