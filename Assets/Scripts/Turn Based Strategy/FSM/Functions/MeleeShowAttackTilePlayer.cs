using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShowAttackTilePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        MeleeShowAttackRangeBehaviour.OnMeleeShowAttackRangeEnter += MeleeShowAttackRangeEnter;
    }
    private void OnDisable()
    {
        MeleeShowAttackRangeBehaviour.OnMeleeShowAttackRangeEnter -= MeleeShowAttackRangeEnter;
    }
    private void MeleeShowAttackRangeEnter()
    {
        var cellSize = TileManager.CellSize;

        var IsCharacter = !(_targetEntity.GetComponent("Character") as Entity is null);
        if (IsCharacter)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    var position = new Vector3Int(i, j, 0);
                    var currentGridPosition = _targetGridPosition + position;
                    var currentGridCenterPosition = currentGridPosition + cellSize;

                    var IsNothingOrIsEnemyCharacter = (InTile(currentGridCenterPosition) == (int)EntityType.Nothing ||
                        InTile(currentGridCenterPosition) == (int)EntityType.EnemyCharacter) && !_collisionTilemap.HasTile(currentGridPosition);
                    if (_uITilemap.HasTile(currentGridPosition))
                    {
                        if (IsNothingOrIsEnemyCharacter)
                        {
                            _uITilemap.SetTile(currentGridPosition, _targetTile);
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < _enemyHeroAttackableTiles.Count; x++)
            {
                var currentGridPosition = _enemyHeroAttackableTiles[x];
                var currentGridCenterPosition = currentGridPosition + cellSize;

                var IsNothingOrIsEnemy = InTile(currentGridCenterPosition) == (int)EntityType.Nothing ||
                    InTile(currentGridCenterPosition) == (int)EntityType.EnemyCharacter || InTile(currentGridCenterPosition) == (int)EntityType.EnemyHero;

                if (_floorTilemap.HasTile(currentGridPosition))
                {
                    if (IsNothingOrIsEnemy)
                    {
                        _uITilemap.SetTile(currentGridPosition, _targetTile);
                    }
                }
            }
            //!(i == 0 && j == 0) && _uITilemap.HasTile(currentGridPosition) && (currentGridPosition == _executorGridPos || !(InTile(currentGridCenterPosition) == (int)EntityType.AllyCharacter)) && !_collisionTilemap.HasTile(currentGridPosition)
        }
    }
}
