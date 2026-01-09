using UnityEngine;

public interface IDamageable
{
    void GetDamaged(int damage);
}

public interface ITargetor
{
    void SetTarget(Target target);
    Team GetTeam();

}

public interface ITarget 
{
    EntityData GetData();
    Vector3 GetPosition();
    IHealthSystem GetHealthSystem();
    Team GetTeam();
}