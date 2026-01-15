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
    EntityData GetData();
    Vector3 GetPosition();
    IHealthSystem GetHealthSystem();
    Team GetTeam();
}

public interface ITarget<out TData> : ITarget where TData : EntityData
{
    new TData GetData(); 
}