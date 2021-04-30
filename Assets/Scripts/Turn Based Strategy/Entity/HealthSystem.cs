using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthSystem
{
    public static void TakeDamage(float amount, Entity target = null)
    {
        if (!target)
            target = EntityManager.TargetCharacter;

        var currentHealth = target.HP;

        if (currentHealth - amount <= 0.0f)
        {
            OnDeath();
            if (currentHealth - amount < 0.0f)
                amount = (float)OnOverkill(currentHealth);
        }
        currentHealth -= amount;

        OnHit((int)amount);

        target.ChangeHealth();

        target.HP = currentHealth;
    }
    private static void OnDeath()
    {
        var target = EntityManager.TargetCharacter;
        target.Dead = true;
    }
    private static float OnOverkill(float currentHealth)
    {
        return currentHealth;
    }
    private static void OnHit(int damage)
    {
        var target = EntityManager.TargetCharacter;
        var executor = EntityManager.ExecutorCharacter;
    }
}
