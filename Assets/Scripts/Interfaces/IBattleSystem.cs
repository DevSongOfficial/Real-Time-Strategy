using UnityEngine;

public interface IAttackable
{
    void Attack(IDamageable damageable);
}

public interface IDamageable
{
    void GetDamaged(int damage);
}

public interface ITargetor
{
    void SetTarget(Target target);
}