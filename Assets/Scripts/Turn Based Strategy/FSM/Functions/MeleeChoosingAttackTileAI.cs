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
        if (IsTargetNeighbour())
        {
            _tileChosenGridPosition = _executorGridPosition;
            _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
            animator.SetTrigger("TileChosen");
            animator.SetBool("Attacking", true);
        }
        else
        {
            GetPossiblePosition();
            possiblePosition = OnRangeTiles();

            foreach(Vector3Int i in possiblePosition)
            {
                Debug.Log(i);
            }

            if (possiblePosition.Count == 0)
            {
                _notPossibleTarget.Add(_targetGridPosition);
                animator.SetBool("PreparingAttack", false);
            }
            else
            {
                if (!(_targetEntity.GetComponent("Entity") as Entity is null))
                {
                    _uITilemap.SetTile(_targetGridPosition, _targetTile);
                }
                if (!(_targetEntity.GetComponent("Hero") as Hero is null))
                {
                    ShowHeroTiles();
                }

                _tileChosenGridPosition = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
                _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
                animator.SetTrigger("TileChosen");
                animator.SetBool("Attacking", true);
                _notPossibleTarget.Clear();
            }
            possiblePosition.Clear();
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

                if (ThereIsNothing)
                {
                    possiblePosition.Add(currentGridPosition);
                }
            }
        }
    }
    private bool IsTargetNeighbour()
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
    private List<Vector3Int> OnRangeTiles()
    {
        List<Vector3Int> tempList = new List<Vector3Int>();
        var cellSize = TileManager.CellSize;
        var movementRange = _executorCharacter.Range;
        int counter = 0;

        for (int j = -movementRange; j <= movementRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -movementRange; i <= movementRange; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _executorGridPosition + position;

                var OnAttackRange = Mathf.Abs(i) < counter;
                if (OnAttackRange)
                {
                    if (possiblePosition.Contains(currentGridPosition))
                    {
                        tempList.Add(currentGridPosition);
                    }

                }
            }
        }
        return tempList;
    }
}
