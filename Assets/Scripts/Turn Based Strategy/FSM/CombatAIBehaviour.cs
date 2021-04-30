using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : CombatBehaviour
{
    public GameObject EnemyHeroeGO;
    private List<Vector3> _notPossibleTarget = new List<Vector3>();

    private void OnEnable()
    {
        SelectingBehaviour.OnSelectingEnter += SelectingEnter;
        SelectingBehaviour.OnSelectingUpdate += SelectingUpdate;
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileEnter += Melee_ChoosingTileEnter;
        RangedChoosingTileBehaviour.OnRangedChoosingTileEnter += Ranged_ChoosingTileEnter;
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileEnter += Melee_ChoosingAttackTileEnter;
    }

    private void OnDisable()
    {
        SelectingBehaviour.OnSelectingEnter -= SelectingEnter;
        SelectingBehaviour.OnSelectingUpdate -= SelectingUpdate;
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileEnter -= Melee_ChoosingTileEnter;
        RangedChoosingTileBehaviour.OnRangedChoosingTileEnter -= Ranged_ChoosingTileEnter;
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileEnter -= Melee_ChoosingAttackTileEnter;
    }
    private void Start()
    {
        _enemyHero = EnemyHeroeGO.GetComponent<Hero>();
        _enemyHeroTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-8, 0, 0), new Vector3Int(-7, -1, 0), new Vector3Int(-8, -1, 0) };
        _enemyHeroAttackableTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-7, -1, 0) };
    }
    //----------------------------------------Selecting----------------------------------------
    private void SelectingEnter(Animator animator)
    {
        var characters = EntityManager.GetActiveCharacters(Team.TeamAI);
        while (characters.Length == 0)
        {
            characters = EntityManager.GetActiveCharacters(Team.TeamAI);
        }
        EntityManager.SetExecutor(characters[0]);

        var _currentGridPos = _floorTilemap.WorldToCell(characters[0].transform.position);

        if (characters[0].Class == Class.Ranged)
        {
            animator.SetBool("Ranged", true);
        }
        else
        {
            animator.SetBool("Ranged", false);
        }
        animator.SetBool("Selected", true);

        _executorGridPos = _currentGridPos;
        _uITilemap.SetTile(_executorGridPos, _allyTile);
    }
    private void SelectingUpdate(Animator animator)
    {
        if (!TurnManager.IsAttackRound)
        {
            animator.SetBool("NotWaiting", false);
        }
    }
    //----------------------------------------Melee_ChoosingTile----------------------------------------
    private void Melee_ChoosingTileEnter(Animator animator)
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
        var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorCharacter.Range + 2;
        int counter = 0;

        Vector3Int minPos = new Vector3Int(range + 1, range + 1, 0);

        for (int j = -range; j <= range; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -range; i <= range; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _executorGridPos + vector;
                Vector3 vToW = pos + v;

                if (Mathf.Abs(i) < counter)
                {
                    if (i != -range && i != range && j != -range && j != range)
                    {
                        if (!(_notPossibleTarget.Contains(pos)))
                        {
                            if (InTile(vToW) == (int)EntityType.EnemyHero && _enemyHeroAttackableTiles.Contains(pos))
                            {
                                _targetGridPosition = pos;
                                targetOnRange = true;
                                break;
                            }
                            if (InTile(vToW) == (int)EntityType.EnemyCharacter)
                            {
                                if (minPos.x + minPos.y > Mathf.Abs(i) + Mathf.Abs(j))
                                {
                                    minPos = new Vector3Int(Mathf.Abs(i), Mathf.Abs(j), 0);
                                    _targetGridPosition = pos;
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
        var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorCharacter.Range;
        var counter = 0;
        var min = Vector3.Distance(_executorGridPos, _enemyHero.transform.position);

        for (int j = -range; j <= range; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -range; i <= range; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _executorGridPos + vector;
                Vector3 vToW = pos + v;

                if (Mathf.Abs(i) < counter - 1)
                {
                    if (!(InTile(vToW) == (int)EntityType.EnemyCharacter || InTile(vToW) == (int)EntityType.AllyCharacter || _collisionTilemap.HasTile(pos) || !_floorTilemap.HasTile(pos)))
                    {
                        var currentMin = Vector3.Distance(vToW, _enemyHero.transform.position);
                        if (min > currentMin)
                        {
                            min = currentMin;
                            _tileChosenGridPosition = pos;
                        }
                    }
                }
            }
        }
        _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
        animator.SetTrigger("TileChosen");
    }

    //----------------------------------------Ranged_ChoosingTile----------------------------------------
    private void Ranged_ChoosingTileEnter(Animator animator)
    {
        var characters = EntityManager.GetLivingCharacters(Team.TeamPlayer);

        Debug.Log(characters[0].name);
        EntityManager.SetTarget(characters[0]);
        _targetGridPosition = _floorTilemap.WorldToCell(characters[0].transform.position);
        _uITilemap.SetTile(_targetGridPosition, _targetTile);

        _tileChosenGridPosition = _executorGridPos;

        animator.SetBool("Attacking", true);
        animator.SetTrigger("TileChosen");
    }
    //----------------------------------------Melee_ChoosingAttackTile----------------------------------------
    private void Melee_ChoosingAttackTileEnter(Animator animator)
    {
        if (IsEnemyNeighbour())
        {
            _tileChosenGridPosition = _executorGridPos;
        }
        else
        {
            List<Vector3Int> possiblePos = new List<Vector3Int>();

            var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = _targetGridPosition + vector;
                    Vector3 vToW = pos + v;

                    if (!(i == 0 && j == 0) && _floorTilemap.HasTile(pos) && !_collisionTilemap.HasTile(pos) && InTile(vToW) == (int)EntityType.Nothing)
                    {
                        possiblePos.Add(pos);
                    }
                }
            }
            if (possiblePos.Count == 0)
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
                _tileChosenGridPosition = possiblePos[UnityEngine.Random.Range(0, possiblePos.Count)];
            }
        }
        _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
        animator.SetTrigger("TileChosen");
        animator.SetBool("Attacking", true);
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