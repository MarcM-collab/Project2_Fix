using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilty : Abilty
{
    public int healthAmount;
    public override void Excecute()
    {
        float prevHP = EntityManager.ExecutorCharacter.HP;
        HealthSystem.TakeDamage(-healthAmount, EntityManager.ExecutorCharacter);

        if (prevHP != EntityManager.ExecutorCharacter.HP)
            executed = true;
    }
}
