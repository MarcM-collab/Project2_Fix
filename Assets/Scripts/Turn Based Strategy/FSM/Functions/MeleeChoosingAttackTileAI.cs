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
            _tileChosenGridPosition = _executorGridPosition;
            _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
            animator.SetTrigger("TileChosen");
            animator.SetBool("Attacking", true);
        }
        else
        {
            GetPossiblePosition();

            if (possiblePosition.Count == 0)
            {
                _notPossibleTarget.Add(_targetGridPosition);
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
                foreach (Vector3Int i in possiblePosition)
                {
                    Debug.Log(i + TileManager.CellSize);
                }
                _tileChosenGridPosition = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
                possiblePosition.Clear();
                _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
                animator.SetTrigger("TileChosen");
                animator.SetBool("Attacking", true);
            }
        }
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

                var ThereIsNothing = _floorTilemap.HasTile(currentGridPosition) && !_collisionTilemap.HasTile(currentGridPosition) && InTile(currentGridCenterPosition) == (int)EntityType.Nothing;

                var currentDistance = Mathf.Abs(Vector3.Distance(_executorGridPosition + cellSize, currentGridCenterPosition));

                if (currentDistance < _maxDistance)
                {
                    if (ThereIsNothing)
                    {
                        possiblePosition.Add(currentGridPosition);
                    }
                }
            }
        }
    }
    private bool IsEnemyNeighbour()
    {
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _executorGridPosition + position;

                if (_targetGridPosition == currentGridPosition)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
