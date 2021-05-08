using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : CombatBehaviour
{
    protected static List<Vector3> _notPossibleTarget = new List<Vector3>();
    private void Start()
    {
        _enemyHero = EntityManager.GetHero(Team.TeamPlayer);
        _enemyHeroAttackableTiles = new List<Vector3Int> { new Vector3Int(-5, -1, 0), new Vector3Int(-5, 0, 0)};
    }
}