using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public Abilty ability;
    public void Use()
    {
        if (ability.whiskasCost <= TurnManager.currentMana)
        {
            ability.Excecute();
            TurnManager.SubstractMana(ability.whiskasCost);
        }
    }
}
