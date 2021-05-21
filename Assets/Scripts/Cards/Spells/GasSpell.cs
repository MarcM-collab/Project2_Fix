using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasSpell : Spell
{
    public GameObject GasSpellPrefab;
    public int damage;
    private Team currentTurn;
    private void Update()
    {
        if (activated)
        {
            Vector3Int mouseIntPos = GetIntPos(GetMousePosition);

            if (tileManager.FloorTilemap.HasTile(mouseIntPos))
            {
                if (prevPos != mouseIntPos)
                {
                    tileManager.UITilemap.SetTile(prevPos, null);
                    prevPos = mouseIntPos;
                    tileManager.UITilemap.SetTile(mouseIntPos, tileManager.PointingTile);
                }
            }
        }
    }

    public override void ExecuteSpell()
    {
        base.ExecuteSpell();
        Vector3Int pos = GetIntPos(GetMousePosition);

        if (tileManager.FloorTilemap.HasTile(pos))
        {
            Instantiate(GasSpellPrefab, pos, Quaternion.identity);
            executed = true;
        }
        tileManager.UITilemap.SetTile(prevPos, null);
    }
    public override void IAUse()
    {

    }
}
