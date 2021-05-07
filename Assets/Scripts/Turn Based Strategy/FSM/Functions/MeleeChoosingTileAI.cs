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

        var max = new Vector3(_executorGridPosition.x + _executorCharacter.Range + 2 + cellSize.x, _executorGridPosition.y + cellSize.y, 0);
        _maxDistance = Mathf.Abs(Vector3.Distance(_executorGridPosition + cellSize, max));
        var minDistance = _maxDistance;
        for (int j = -attackRange; j <= attackRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -attackRange; i <= attackRange; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _executorGridPosition + position;
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
                                var currentMin = Mathf.Abs(Vector3.Distance(_executorGridPosition + cellSize, currentGridCenterPosition));
                                if (minDistance > currentMin)
                                {
                                    minDistance = currentMin;
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
        var cellSize = TileManager.CellSize;
        Vector2 vector2 = new Vector2(_targetGridPosition.x + cellSize.x, _targetGridPosition.y + cellSize.y);
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
                EntityManager.SetTarget(hitCollider.gameObject.GetComponent<Entity>());
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
        var min = Vector3.Distance(_executorGridPosition, _enemyHero.transform.position);

        for (int j = -movementRange; j <= movementRange; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -movementRange; i <= movementRange; i++)
            {
                var position = new Vector3Int(i, j, 0);
                var currentGridPosition = _executorGridPosition + position;
                var currentGridCenterPosition = currentGridPosition + cellSize;

                var OnMovementRange = Mathf.Abs(i) < counter;
                if (OnMovementRange)
                {
                    var IsEmptyFloorTile = InTile(currentGridCenterPosition) == (int)EntityType.Nothing 
                        && _floorTilemap.HasTile(currentGridPosition) && !_collisionTilemap.HasTile(currentGridPosition);
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
