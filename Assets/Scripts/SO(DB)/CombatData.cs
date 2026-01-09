using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/Combat Data")]
public class CombatData : ScriptableObject
{
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }
    [field: SerializeField] public string Animation { get; private set; }
    [field:SerializeField] public float WindupTime {get;private set; }

}
