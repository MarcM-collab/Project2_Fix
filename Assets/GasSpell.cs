using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpell : Spell
{
    public GameObject GasSpellPrefab;
    public int damage;
    private Team currentTurn;
    public override void ExecuteSpell()
    {
        Instantiate(GasSpellPrefab, GetMousePosition, Quaternion.identity);
        executed = true;
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
}
