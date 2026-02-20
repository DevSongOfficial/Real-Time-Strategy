using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Entity Data")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public Transform Prefab { get; private set; }
    [field: Space]
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite ProfileSprite { get; private set; }
    [field: SerializeField] public int RadiusOnTerrain { get; private set; } // For selection indicator & offset when moving towards the target.
    [field: SerializeField] public Vector3 SelectionIndicatorPositionOffset { get; private set; }

    [field: Header("Commands")]
    [field: SerializeField] public List<CommandData> CommandSet { get; private set; }

    [field: Header("Status")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public CombatData Combat { get; private set; }
    [field: SerializeField] public MovementData Movement { get; private set; }
}