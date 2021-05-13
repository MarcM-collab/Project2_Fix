using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthSystem
{
    public static void TakeDamage(int amount, Entity target = null)
    {
        if (!target)
            target = EntityManager.TargetCharacter;

        var currentHealth = target.HP;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnDeath(target);
            if (amount < 0)
                amount = OnOverkill(currentHealth);
        }

        target.ChangeHealth();

        target.HP = currentHealth;
    }
    private static void OnDeath(Entity toKill)
    {
        toKill.Dead = true;
    }
    private static int OnOverkill(int currentHealth)
    {
        return currentHealth;
    }
    private static void OnHit(int damage)
    {
        var target = EntityManager.TargetCharacter;
        var executor = EntityManager.ExecutorCharacter;
    }
}
