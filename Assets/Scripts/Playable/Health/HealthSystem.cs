using System;

public interface IHealthSystem
{
    int CurrentHealth { get; }
    int MaxHealth { get; }


    event Action OnDie;
    event Action OnHelathChanged;
    event Action<int> OnDamaged;
    event Action<int> OnHealed;
}

public class HealthSystem : IHealthSystem
{
    public int CurrentHealth => health;
    public int MaxHealth => maxHealth;

    private int health;
    private int maxHealth;

    public event Action OnDie;
    public event Action OnHelathChanged;
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;
    
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void GetDamaged(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        OnDamaged?.Invoke(damage);
        OnHelathChanged?.Invoke();


        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        OnHealed?.Invoke(amount);
        OnHelathChanged?.Invoke();

        if (health > maxHealth) 
            health = maxHealth;
    }

    private void Die()
    {
        OnDie?.Invoke();
    }
}
