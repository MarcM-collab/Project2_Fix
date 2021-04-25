using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthSystem
{
    public static void TakeDamage(float amount)
    {
        var currentHealth = CharacterManager.TargetCharacter.HP;

        if (currentHealth - amount <= 0.0f)
        {
            OnDeath();
            if (currentHealth - amount < 0.0f)
                amount = (float)OnOverkill(currentHealth);
        }
        currentHealth -= amount;

        OnHit((int)amount);

        CharacterManager.TargetCharacter.ChangeHealth();

        CharacterManager.TargetCharacter.HP = currentHealth;
    }
    private static void OnDeath()
    {
        var target = CharacterManager.TargetCharacter;
        target.Dead = true;
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
