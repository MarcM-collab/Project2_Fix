using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingTileAI : CombatAIBehaviour
{
    private void OnEnable()
    {
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileEnter += MeleeChoosingTileEnter;
    }
    private void OnDisable()
    {
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileEnter -= MeleeChoosingTileEnter;
    }
    private void MeleeChoosingTileEnter(Animator animator)
    {
        var targetOnRange = false;

        FindTarget(ref targetOnRange);

        if (targetOnRange)
        {
            SetTarget(animator);
        }
        else
        {
            MoveToTile(animator);
        }
    }
    private void FindTarget(ref bool targetOnRange)
    {
        var cellSize = TileManager.CellSize;
        var attackRange = _executorCharacter.Range + 2;
        int counter = 0;

        Vector3Int minPos = new Vector3Int(attackRange + 1, attackRange + 1, 0);

        for (int j = -attackRange; j <= attackRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -attackRange; i <= attackRange; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _targetGridPosition + position;
                var currentGridCenterPosition = currentGridPosition + cellSize;

                var OnAttackRange = Mathf.Abs(i) < counter;
                if (OnAttackRange)
                {
                    if (i != -attackRange && i != attackRange && j != -attackRange && j != attackRange)
                    {
                        var IsAPossiblePositionToAttack = !(_notPossibleTarget.Contains(currentGridPosition));
                        if (IsAPossiblePositionToAttack)
                        {
                            var HeroOnRange = _enemyHeroAttackableTiles.Contains(currentGridPosition);
                            if (HeroOnRange)
                            {
                                //Target Hero
                                _targetGridPosition = currentGridPosition;
                                targetOnRange = true;
                                break;
                            }
                            var EnemyOnRange = InTile(currentGridCenterPosition) == (int)EntityType.EnemyCharacter;
                            if (EnemyOnRange)
                            {
                                //Take the nearest enemy
                                if (minPos.x + minPos.y > Mathf.Abs(i) + Mathf.Abs(j))
                                {
                                    minPos = new Vector3Int(Mathf.Abs(i), Mathf.Abs(j), 0);
                                    _targetGridPosition = currentGridPosition;
                                    targetOnRange = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void SetTarget(Animator animator)
    {
        var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        Vector2 vector2 = new Vector2(_targetGridPosition.x + v.x, _targetGridPosition.y + v.y);
        RaycastHit2D hit = Physics2D.Raycast(vector2, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            if (!(hitCollider.gameObject.GetComponent("Entity") as Entity is null))
            {
                EntityManager.SetTarget(hitCollider.gameObject.GetComponent<Entity>());
                _uITilemap.SetTile(_targetGridPosition, _targetTile);
            }
            if (!(hitCollider.gameObject.GetComponent("Hero") as Hero is null))
            {
                ShowHeroTiles();
            }
            animator.SetBool("PreparingAttack", true);
        }
    }
    private void MoveToTile(Animator animator)
    {
        var cellSize = TileManager.CellSize;
        var movementRange = _executorCharacter.Range;
        var counter = 0;
        var min = Vector3.Distance(_executorGridPos, _enemyHero.transform.position);

        for (int j = -movementRange; j <= movementRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -movementRange; i <= movementRange; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _targetGridPosition + position;
                var currentGridCenterPosition = currentGridPosition + cellSize;

                var OnMovementRange = Mathf.Abs(i) < counter - 1;
                if (OnMovementRange)
                {
                    var IsEmptyFloorTile = InTile(currentGridCenterPosition) == (int)EntityType.Nothing 
                        && _collisionTilemap.HasTile(currentGridPosition) && _floorTilemap.HasTile(currentGridPosition);
                    if (IsEmptyFloorTile)
                    {
                        //Take the tile nearest to the Player Hero
                        var currentMin = Vector3.Distance(currentGridCenterPosition, _enemyHero.transform.position);
                        if (min > currentMin)
                        {
                            min = currentMin;
                            _tileChosenGridPosition = currentGridPosition;
                        }
                    }
                }
            }
        }
        _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
        animator.SetTrigger("TileChosen");
    }
}
