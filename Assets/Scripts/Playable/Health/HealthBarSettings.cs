using UnityEngine;

[CreateAssetMenu(fileName = "HealthBarSettings", menuName = "Scriptable Objects/HealthBarSettings")]
public class HealthBarSettings : ScriptableObject
{
    [field: SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public Vector2 Size { get; private set; } = new Vector2(100, 20);

    [Header("Health Bar Color")]
    [field: SerializeField] public Color Color_RedTeam { get; private set; }
    [field: SerializeField] public Color Color_GreenTeam { get; private set; }
    [field: SerializeField] public Color Color_BlueTeam { get; private set; }
}