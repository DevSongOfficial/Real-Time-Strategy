using UnityEngine;

[CreateAssetMenu(fileName = "HealthBarSettings", menuName = "Scriptable Objects/HealthBarSettings")]
public class HealthBarSettings : ScriptableObject
{
    [field: SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public Vector2 Size { get; private set; } = new Vector2(100, 20);
}