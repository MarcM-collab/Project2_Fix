using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedShowRangePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        RangedShowRangeBehaviour.OnRangedShowRangeEnter += RangedShowRangeEnter;
    }
    private void OnDisable()
    {
        RangedShowRangeBehaviour.OnRangedShowRangeEnter -= RangedShowRangeEnter;
    }
    private void RangedShowRangeEnter()
    {
        var cellSize = TileManager.CellSize;
        var attackRange = _executorCharacter.Range;
        var counter = 0;

        for (int j = -attackRange; j <= attackRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -attackRange; i <= attackRange; i++)
            {
                var IsMovementRange = Mathf.Abs(i) < counter;

                if (IsMovementRange)
                {
                    var position = new Vector3Int(i, j, 0);
                    var currentGridPosition = _executorGridPosition + position;
                    var currentGridCenterPosition = currentGridPosition + cellSize;

                    var ThereIsAnAlly = InTile(currentGridCenterPosition) == (int)EntityType.AllyCharacter;
                    var ThereIsNothing = CanMove(currentGridPosition) && InTile(currentGridCenterPosition) == (int)EntityType.Nothing;
                    var ThereIsACollider = _collisionTilemap.HasTile(currentGridPosition);

                    if (_floorTilemap.HasTile(currentGridPosition))
                    {
                        if (_executorGridPosition == currentGridPosition)
                            _uITilemap.SetTile(currentGridPosition, _pointingTile);
                        else if (ThereIsNothing)
                            _uITilemap.SetTile(currentGridPosition, _allyTile);
                        else if (ThereIsACollider || ThereIsAnAlly)
                        {
                            _uITilemap.SetTile(currentGridPosition, _collisionAllyTile);
                        }
                    }
                }
            }
        }

        TileManager.ShowTilesInTilemap(_floorTilemap,_uITilemap, _targetTile, IsEnemy);
    }
    private bool IsEnemy(Vector3Int vector)
    {
        var cellSize = TileManager.CellSize;
        var currentGridCenterPosition = vector + cellSize;
        return InTile(currentGridCenterPosition) == (int)EntityType.EnemyCharacter;
    }
}
