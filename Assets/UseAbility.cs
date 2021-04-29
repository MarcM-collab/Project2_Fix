using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public TurnManager TurnManager;
    public Abilty ability;
    public void Use()
    {
        if (ability.whiskasCost <= TurnManager.currentMana)
        {
            ability.Excecute();
            TurnManager.SubstractMana(ability.whiskasCost);
            print("health");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            HealthSystem.TakeDamage(2, EntityManager.ExecutorCharacter);
            print("Damaged!");
        }
    }
}
