using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void Attack();
}

public class AttackHandler
{
    private IEnumerable<IAttackable> attackables;

    public AttackHandler(IEnumerable<IAttackable> attackables)
    {
        this.attackables = attackables;
    }

    public void HandleAttack()
    {
        if (!Input.GetKeyDown(KeyCode.A)) return;

        foreach (var attackable in attackables) { attackable.Attack(); }
    }
}