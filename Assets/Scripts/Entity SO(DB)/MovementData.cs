using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/Movement Data")]
public class MovementData : ScriptableObject
{
    [field: SerializeField] public string IdleAnimation { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public string RunningAnimation { get; private set; }

}
