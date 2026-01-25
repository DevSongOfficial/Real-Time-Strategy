using UnityEngine;

[CreateAssetMenu(fileName = "SelectTarget", menuName = "Scriptable Objects/Command/Select Target")]
public class SelectTargetCommandData : CommandData
{
    public override CommandType Type => CommandType.Utility;

}
