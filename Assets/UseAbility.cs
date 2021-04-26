using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    public Abilty ability;
    public void Use()
    {
        print("Health!!");
        ability.Excecute();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            HealthSystem.TakeDamage(2, CharacterManager.ExecutorCharacter);
            print("Damaged!");
        }
    }
}
