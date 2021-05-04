using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingAttackTileAI : CombatAIBehaviour
{
    private List<Vector3Int> possiblePosition = new List<Vector3Int>();
    private void OnEnable()
    {
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileEnter += MeleeChoosingAttackTileEnter;
    }
    private void OnDisable()
    {
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileEnter -= MeleeChoosingAttackTileEnter;
    }
    private void MeleeChoosingAttackTileEnter(Animator animator)
    {
        if (IsEnemyNeighbour())
        {
            _tileChosenGridPosition = _executorGridPos;
        }
        else
        {
            GetPossiblePosition();

            if (possiblePosition.Count == 0)
            {
                _notPossibleTarget.Add(_targetGridPosition);
                animator.SetBool("Selected", false);
                animator.SetBool("PreparingAttack", false);

                if (!(_targetEntity.GetComponent("Entity") as Entity is null))
                {
                    _uITilemap.SetTile(_targetGridPosition, null);
                }
                if (!(_targetEntity.GetComponent("Hero") as Hero is null))
                {
                    HideHeroTiles();
                }
            }
            else
            {
                _tileChosenGridPosition = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
            }
        }
        _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
        animator.SetTrigger("TileChosen");
        animator.SetBool("Attacking", true);
    }
    private void GetPossiblePosition()
    {
        var cellSize = TileManager.CellSize;
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _targetGridPosition + position;
                var currentGridCenterPosition = currentGridPosition + cellSize;

                var ThereIsNothing = !(i == 0 && j == 0) && _floorTilemap.HasTile(currentGridPosition)
                    && !_collisionTilemap.HasTile(currentGridPosition) && InTile(currentGridCenterPosition) == (int)EntityType.Nothing;
                if (ThereIsNothing)
                {
                    possiblePosition.Add(currentGridPosition);
                }
            }
        }
    }
    private bool IsEnemyNeighbour()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _executorGridPos + vector;
                Vector3 vToW = pos + v;

                if (InTile(vToW) == (int)EntityType.EnemyCharacter || InTile(vToW) == (int)EntityType.EnemyHero)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
