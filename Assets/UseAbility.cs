using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public Abilty ability;
    public void Use()
    {
        if (ability.whiskasCost <= TurnManager.currentMana && TurnManager.TeamTurn == Team.TeamPlayer)
        {
            ability.Excecute();
        }
    }
    private void Update()
    {
        if (ability.executed)
        {
            EntityManager.ExecutorCharacter.Exhausted = true;
            TurnManager.SubstractMana(ability.whiskasCost);
            ability.executed = false;
        }
    }
}
