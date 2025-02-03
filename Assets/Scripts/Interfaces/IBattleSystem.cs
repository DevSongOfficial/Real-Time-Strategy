using UnityEngine;

public interface IDamageable
{
    void GetDamaged(int damage);
}

public interface ITargetor
{
    void SetTarget(Target target);
}

public interface ITarget 
{
    Vector3 GetPosition();
    IHealthSystem GetHealthSystem();
}