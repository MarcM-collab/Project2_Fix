using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebilitySpell : Spell
{
    private bool executing = false;
    private bool used = false; //temporal
    public override void ExecuteSpell()
    {
        executing = true;
    }
    public override void IAUse()
    {
        //Pick an allie char
        Character[] chars = FindObjectsOfType<Character>();
        ReduceAttack(GetHighestAttackUnit(chars));
    }

    private Character GetHighestAttackUnit(Character[] allCharactersInScene)
    {
        Character priorChar = null;
        for (int i = 0; i < allCharactersInScene.Length; i++)
        {
            if (allCharactersInScene[i].Team == Team.TeamPlayer)
            {
                if (!priorChar || priorChar.AttackPoints < allCharactersInScene[i].AttackPoints)
                {
                    priorChar = allCharactersInScene[i];
                }
            }
        }
        return priorChar;
    }

    private void Update()
    {
        if (executing)
        {
            if (Input.GetMouseButton(0) &&!used)
            {
                RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);

                if (hit2D)
                {

                    if (hit2D.transform.CompareTag("Character"))
                    {
                        used = true;
                        Character target = hit2D.collider.gameObject.GetComponent<Character>();
                        ReduceAttack(target);
                    }
                    else
                    {
                        executing = false;
                    }
                }

            }
        }
    }
    private void ReduceAttack(Character target)
    {
        target.AttackPoints /= 2;

        if (target.AttackPoints <= 0)
            target.AttackPoints = 1;

        int example = target.AttackPoints;
        print("Attack of the unit reduced: " + example);
    }
}
