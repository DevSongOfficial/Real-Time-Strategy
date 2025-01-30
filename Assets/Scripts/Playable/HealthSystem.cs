using System;
using UnityEngine;

public class HealthSystem
{
    private int health;
    private int maxHealth;

    public event Action OnDie;
    
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void GetDamaged(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth) 
            health = maxHealth;
    }

    private void Die()
    {
        OnDie?.Invoke();
    }
}
