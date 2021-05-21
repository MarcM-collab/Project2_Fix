using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealAbility : Abilty
{
    public int damage;
    private Character selfChar;
    private void Awake()
    {
        selfChar = gameObject.GetComponent<Character>();
    }
    public override void Excecute()
    {
        DoAction(Team.TeamAI);
    }
    public override void IAExecute()
    {
        DoAction(Team.TeamPlayer);
    }
    private void DoAction(Team targetTeam)
    {
        Character[] characters = EntityManager.GetCharacters(targetTeam);
        if (characters.Length > 0)
        {
            EntityManager.SetTarget(characters[Random.Range(0, characters.Length)]);
            EntityManager.SetExecutor(selfChar);

            EntityManager.ExecutorCharacter.GetComponent<Character>().currentAttack = damage;

            EntityManager.TargetCharacter.Hit = true;

            //EntityManager.SetTarget(selfChar);
            if (EntityManager.ExecutorCharacter.HP + damage <= EntityManager.ExecutorCharacter.MaxHP) //calcular diferencia (ej si tienes 4 hp i el max es 5 deberias curarte 1)
            {
                //    EntityManager.SetTarget(selfChar);
                //    EntityManager.TargetCharacter.Hit = true;
                HealthSystem.Heal(EntityManager.ExecutorCharacter, damage);
            }

            executed = true;
        }
    }
}
