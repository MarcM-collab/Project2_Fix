using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthSystem
{
    public static void TakeDamage(float amount)
    {
        var currentHealth = EntityManager.TargetEntity.HP;

        if (currentHealth - amount <= 0.0f)
        {
            OnDeath();
            if (currentHealth - amount < 0.0f)
                amount = (float)OnOverkill(currentHealth);
        }
        currentHealth -= amount;

        OnHit((int)amount);

        EntityManager.TargetEntity.ChangeHealth();

        EntityManager.TargetEntity.HP = currentHealth;
    }
    private static void OnDeath()
    {
        var target = EntityManager.TargetEntity;
    }
    private static float OnOverkill(float currentHealth)
    {
        return currentHealth;
    }
    private static void OnHit(int damage)
    {
        var target = EntityManager.TargetEntity;
        var executor = EntityManager.ExecutorEntity;
    }
}
