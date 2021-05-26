using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingTileAI : CombatAIBehaviour
{
    List<Vector3Int> attackableCharactersTiles = new List<Vector3Int>();
    private bool _targetOnRange;
    private bool _isHero;
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
        _targetOnRange = false;
        _isHero = false;
        FindTarget();

        if (_targetOnRange)
        {
            SetTarget(animator);
            attackableCharactersTiles.Clear();
        }
        else
        {
            MoveToTile(animator);
        }
    }
    private void FindTarget()
    {
        var cellSize = TileManager.CellSize;
        var attackRange = _executorCharacter.Range + 2;
        int counter = 0;

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
                                _targetOnRange = true;
                                _isHero = true;
                                break;
                            }
                            var EnemyOnRange = InTile(currentGridCenterPosition) == (int)EntityType.EnemyCharacter;
                            if (EnemyOnRange)
                            {
                                //Take the nearest enemy
                                attackableCharactersTiles.Add(currentGridPosition);
                                _targetOnRange = true;
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
        Entity target;
        if(!_isHero)
        {
            var enemiesWhoDieOnHit = TakeEnemiesWhoDieOnHit(attackableCharactersTiles);
            if (enemiesWhoDieOnHit.Count > 0)
            {
                _targetGridPosition = TakeNearestToExecutor(enemiesWhoDieOnHit);
            }
            else
            {
                _targetGridPosition = TakeNearestToExecutor(attackableCharactersTiles);
            }
        }
        target = GetEntityOnTile(_targetGridPosition);
        EntityManager.SetTarget(target);
        animator.SetBool("PreparingAttack", true);
        
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

    private List<Vector3Int> TakeEnemiesWhoDieOnHit(List<Vector3Int> list)
    {
        List<Vector3Int> enemiesWhoDieOnHit = new List<Vector3Int>();
        foreach (Vector3Int vector3Int in list)
        {
            if (GetEntityOnTile(vector3Int).HP <= _executorCharacter.AttackPoints)
            {
                enemiesWhoDieOnHit.Add(vector3Int);
            }
        }
        return enemiesWhoDieOnHit;
    }

    private Vector3Int TakeNearestToExecutor(List<Vector3Int> list)
    {
        var attackRange = _executorCharacter.Range + 2;
        var max = new Vector3(_executorGridPosition.x + attackRange, _executorGridPosition.y, 0);
        var maxDistance = Mathf.Abs(Vector3.Distance(_executorGridPosition, max));
        var minDistance = maxDistance;
        Vector3Int nearestPosition = Vector3Int.zero;

        foreach (Vector3Int vector3Int in list) 
        {
            var currentMin = Mathf.Abs(Vector3.Distance(_executorGridPosition, vector3Int));
            if (minDistance > currentMin)
            {
                minDistance = currentMin;
                nearestPosition = vector3Int;
            }
        }

        return nearestPosition;
    }

    private Entity GetEntityOnTile(Vector3Int vector3Int)
    {
        var cellSize = TileManager.CellSize;
        var postion = new Vector2(vector3Int.x + cellSize.x, vector3Int.y + cellSize.y);
        RaycastHit2D hit = Physics2D.Raycast(postion, Vector2.zero, Mathf.Infinity);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                return gameObject.GetComponent<Entity>();
            }
        }
        return null;
    }
}
