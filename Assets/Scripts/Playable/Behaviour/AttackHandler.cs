using System.Collections.Generic;
using UnityEngine;

public class AttackHandler
{
    private IEnumerable<IAttackable> attackables;

    private IDamageable target;

    public AttackHandler(IEnumerable<IAttackable> attackables)
    {
        this.attackables = attackables;
    }

    public void HandleAttack()
    {
        if (!Input.GetKeyDown(KeyCode.A)) return;
        
        foreach (var attackable in attackables)
            attackable.Attack(target);
    }
}