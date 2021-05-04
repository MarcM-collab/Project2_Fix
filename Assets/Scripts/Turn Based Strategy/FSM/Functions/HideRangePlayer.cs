using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRangePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        HideRangeBehaviour.OnHideRangeEnter += HideRangeEnter;
    }
    private void OnDisable()
    {
        HideRangeBehaviour.OnHideRangeEnter -= HideRangeEnter;
    }
    private void HideRangeEnter()
    {
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap, null, HideRange);
        IsAttacking = false;
    }
    private bool HideRange(Vector3Int vector)
    {
        var cellSize = TileManager.CellSize;
        //if (IsAttacking)
        //{
        //    return ((InTile(currentGridCenterPosition) != (int)EntityType.EnemyHero && InTile(currentGridCenterPosition) != (int)EntityType.EnemyCharacter)
        //        && vector != _tileChosenGridPosition && vector != _executorGridPos);
        //}
        //else
        //{
        //    return (vector != _tileChosenGridPosition && vector != _executorGridPos);
        //}
        return (vector != _targetGridPosition);
    }
}
