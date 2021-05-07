using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRangePlayer : CombatPlayerBehaviour
{
    private bool IsAttacking;
    private void OnEnable()
    {
        HideRangeBehaviour.OnHideRangeEnter += HideRangeEnter;
    }
    private void OnDisable()
    {
        HideRangeBehaviour.OnHideRangeEnter -= HideRangeEnter;
    }
    private void HideRangeEnter(Animator animator)
    {
        if (animator.GetBool("Attacking"))
        {
            IsAttacking = true;
        }
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap, null, HideRange);
        IsAttacking = false;
    }
    private bool HideRange(Vector3Int vector)
    {
        var cellSize = TileManager.CellSize;
        if (IsAttacking)
        {
            return (vector != _targetGridPosition);
        }
        else
        {
            return true;
        }
    }
}
