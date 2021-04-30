using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebilitySpell : Spell
{
    public override void ExecuteSpell()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);

        if (hit2D)
        {
            if (hit2D.transform.CompareTag("Character"))
            {
                Character target = hit2D.collider.gameObject.GetComponent<Character>();
                ReduceAttack(target);
                executed = true;
            }
        }
    }
    public override void IAUse()
    {
        //Pick an allie char
        Character[] chars = FindObjectsOfType<Character>();
        Character priorChar = GetHighestAttackUnit(chars);
        if (priorChar)
        {
            ReduceAttack(priorChar);
            executed = true;
        }
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
    private void ReduceAttack(Character target)
    {
        target.AttackPoints /= 2;

        if (target.AttackPoints <= 0)
            target.AttackPoints = 1;

        int example = target.AttackPoints;
        print("Attack of the unit reduced: " + example);
    }
}
