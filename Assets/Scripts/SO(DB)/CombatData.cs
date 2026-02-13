using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/Combat Data")]
public class CombatData : ScriptableObject
{
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }
    [field: SerializeField] public string AttackAnimation { get; private set; }
    [field: Tooltip("Ready to attack Animation")]
    [field: SerializeField] public string AttackIdleAnimation { get; private set; } 
    [field:SerializeField] public float WindupTime {get;private set; }

}
