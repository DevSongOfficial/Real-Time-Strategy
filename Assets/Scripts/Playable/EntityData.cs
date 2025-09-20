using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Entity Data")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public Team Team { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public int RadiusOnTerrain { get; private set; } // For selection indicator & offset when moving towards the target.
    [field: SerializeField] public Transform Prefab { get; private set; }

    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }
}