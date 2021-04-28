using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public Abilty ability;
    private TurnManager turn;
    public void Use()
    {
        if (!turn)
            turn = FindObjectOfType<TurnManager>();

        if (ability.whiskasCost <= turn.currentMana)
        {
            ability.Excecute();
            turn.SubstractMana(ability.whiskasCost);
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
