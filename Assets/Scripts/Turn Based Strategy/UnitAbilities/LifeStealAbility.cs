using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealAbility : Abilty
{
    public int damage;
    public override void Excecute()
    {
        DoAction(Team.TeamPlayer);
    }
    public override void IAExecute()
    {
        DoAction(Team.TeamAI);
    }
    private void DoAction(Team team)
    {
        Character[] characters = EntityManager.GetLivingCharacters(team);

        if (characters.Length > 0)
        {
            HealthSystem.TakeDamage(damage, characters[Random.Range(0, characters.Length)]); //damage
            HealthSystem.TakeDamage(-damage, EntityManager.ExecutorCharacter); //heal
            executed = true;
        }
    }
}
