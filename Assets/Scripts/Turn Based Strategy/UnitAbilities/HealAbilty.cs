using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilty : Abilty
{
    public int healthAmount;
    public override void Excecute()
    {
        Character c = EntityManager.ExecutorCharacter;
        if (c.HP + healthAmount <= c.MaxHP)
        {
            HealthSystem.Heal(c, healthAmount);
            executed = true;
        }
    }
}
