using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpell : Spell
{
    public int damage;
    private Team currentTurn;
    public override void ExecuteSpell()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);

        if (hit2D)
        {
            //if (TileManager.)
            //currentTurn = TurnManager.TeamTurn;
        }
    }
    public override void IAUse()
    {

    }

    private void Update()
    {
        if (TurnManager.TeamTurn != currentTurn)
        {
            currentTurn = TurnManager.TeamTurn;
            ApplyEffect();
        }
    }

    private void ApplyEffect()
    {

    }
}
