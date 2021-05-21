using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHideAttackRangePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        MeleeHideAttackRangeBehaviour.OnMeleeHideAttackRangeEnter += MeleeHideAttackRangeEnter;
    }
    private void OnDisable()
    {
        MeleeHideAttackRangeBehaviour.OnMeleeHideAttackRangeEnter -= MeleeHideAttackRangeEnter;
    }
    private void MeleeHideAttackRangeEnter()
    {
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap, _allyTile, IsAttackTile);
    }
    private bool IsAttackTile(Vector3Int vector)
    {
        var cellSize = TileManager.CellSize;
        var currentGridCenterPosition = vector + cellSize;
        return (InTile(currentGridCenterPosition) == (int)EntityType.Nothing && _uITilemap.HasTile(vector)) && !_collisionTilemap.HasTile(vector);
    }
}
