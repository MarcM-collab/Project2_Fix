using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthSystem
{
    public static void TakeDamage(float amount, Character target = null)
    {
        if (!target)
            target = CharacterManager.TargetCharacter;

        var currentHealth = target.HP;

        if (currentHealth - amount <= 0.0f)
        {
            OnDeath();
            if (currentHealth - amount < 0.0f)
                amount = (float)OnOverkill(currentHealth);
        }
        currentHealth -= amount;

        if (target.MaxHP > currentHealth)
            target.HP = target.MaxHP;
        else
            target.HP = currentHealth;

        OnHit((int)amount);

        target.ChangeHealth();



    }
    private static void OnDeath()
    {
        var target = CharacterManager.TargetCharacter;
    }
    private static float OnOverkill(float currentHealth)
    {
        return currentHealth;
    }
    private static void OnHit(int damage)
    {
        var target = CharacterManager.TargetCharacter;
        var executor = CharacterManager.ExecutorCharacter;
    }
}
