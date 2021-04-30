using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public TurnManager turnManager;
    public Abilty ability;
    public void Use()
    {
        if (!turnManager)
            turnManager = FindObjectOfType<TurnManager>();

        if (ability.whiskasCost <= turnManager.currentMana)
        {
            ability.Excecute();
            turnManager.SubstractMana(ability.whiskasCost);
        }

    }
}
