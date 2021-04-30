using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilty : Abilty
{
    public int healthAmount;
    public override void Excecute()
    {
        HealthSystem.TakeDamage(-healthAmount, EntityManager.ExecutorCharacter);
    }
}
