using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public Team Team { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }

    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }





}
