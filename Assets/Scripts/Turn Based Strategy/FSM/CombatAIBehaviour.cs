using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : CombatBehaviour
{
    protected List<Vector3> _notPossibleTarget = new List<Vector3>();

    private void Start()
    {
        _enemyHero = EntityManager.GetHero(Team.TeamPlayer);
        _enemyHeroTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-8, 0, 0), new Vector3Int(-7, -1, 0), new Vector3Int(-8, -1, 0) };
        _enemyHeroAttackableTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-7, -1, 0) };
    }
}