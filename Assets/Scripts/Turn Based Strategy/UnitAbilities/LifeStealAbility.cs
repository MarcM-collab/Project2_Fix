using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealAbility : Abilty
{
    public int damage;
    private Character selfChar;
    private void Start()
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
        print(characters.Length);
        if (characters.Length > 0)
        {
            HealthSystem.TakeDamage(damage, characters[Random.Range(0, characters.Length)]); //damage
            HealthSystem.TakeDamage(-damage, selfChar); //heal
            executed = true;
        }
    }
}
