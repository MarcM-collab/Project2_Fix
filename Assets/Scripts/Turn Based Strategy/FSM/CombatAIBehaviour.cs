using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : CombatBehaviour
{
    public GameObject EnemyHeroeGO;

    private void OnEnable()
    {
        Selecting.OnSelectingEnter += SelectingEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileEnter += Melee_ChoosingTileEnter;
        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter += Ranged_ChoosingTileEnter;
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter += Melee_ChoosingAttackTileEnter;
    }

    private void OnDisable()
    {
        Selecting.OnSelectingEnter -= SelectingEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileEnter -= Melee_ChoosingTileEnter;
        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter -= Ranged_ChoosingTileEnter;
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter -= Melee_ChoosingAttackTileEnter;
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
    //----------------------------------------Melee_ChoosingTile----------------------------------------
    private void Melee_ChoosingTileEnter(Animator animator)
    {
        var targetOnRange = false;

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
                        if (!IsHeroCovered()) 
                        {
                            if (InTile(vToW) == (int)CharType.EnemyHero && _enemyHeroAttackableTiles.Contains(pos))
                            {
                                _targetGridPos = pos;
                                targetOnRange = true;
                                break;
                            }
                        }
                        if (InTile(vToW) == (int)CharType.EnemyCharacter)
                        {
                            if (minPos.x + minPos.y > Mathf.Abs(i) + Mathf.Abs(j))
                            {
                                minPos = new Vector3Int(Mathf.Abs(i), Mathf.Abs(j), 0);
                                _targetGridPos = pos;
                                targetOnRange = true;
                            }
                        }
                    }
                }
            }

        }
        if (targetOnRange)
        {
            Vector2 vector2 = new Vector2(_targetGridPos.x + v.x, _targetGridPos.y + v.y);
            RaycastHit2D hit = Physics2D.Raycast(vector2, Vector2.zero);
            var hitCollider = hit.collider;
            if (hitCollider != null)
            {
                if (!(hitCollider.gameObject.GetComponent("Entity") as Entity is null))
                {
                    EntityManager.SetTarget(hitCollider.gameObject.GetComponent<Entity>());
                    _uITilemap.SetTile(_targetGridPos, _targetTile);
                }
                if (!(hitCollider.gameObject.GetComponent("Hero") as Hero is null))
                {
                    ShowHeroTiles();
                }
                animator.SetBool("PreparingAttack", true);
            }
        }
        else
        {
            range = _executorCharacter.Range;
            counter = 0;
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
                        if (!(InTile(vToW) == (int)CharType.EnemyCharacter || InTile(vToW) == (int)CharType.AllyCharacter || _collisionTilemap.HasTile(pos) || !_floorTilemap.HasTile(pos)))
                        {
                            var currentMin = Vector3.Distance(vToW, _enemyHero.transform.position);
                            if (min > currentMin)
                            {
                                min = currentMin;
                                _tileChosenPos = pos;
                            }
                        }
                    }
                }
            }
            _uITilemap.SetTile(_tileChosenPos, _allyTile);
            animator.SetTrigger("TileChosen");
        }
    }
    
    //----------------------------------------Ranged_ChoosingTile----------------------------------------
    private void Ranged_ChoosingTileEnter(Animator animator)
    {
        var characters = EntityManager.GetLivingCharacters(Team.TeamPlayer);

        Debug.Log(characters[0].name);
        EntityManager.SetTarget(characters[0]);
        _targetGridPos = _floorTilemap.WorldToCell(characters[0].transform.position);
        _uITilemap.SetTile(_targetGridPos, _targetTile);

        _tileChosenPos = _executorGridPos;

        animator.SetBool("Attacking", true);
        animator.SetTrigger("TileChosen");
    }
    //----------------------------------------Melee_ChoosingAttackTile----------------------------------------
    private void Melee_ChoosingAttackTileEnter(Animator animator)
    {
        if (IsEnemyNeighbour())
        {
            _tileChosenPos = _executorGridPos;
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
                    var pos = _targetGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (!(i == 0 && j == 0) && _floorTilemap.HasTile(pos) && !_collisionTilemap.HasTile(pos) && InTile(vToW) == (int)CharType.Nothing)
                    {
                        possiblePos.Add(pos);
                    }
                }
            }

            _tileChosenPos = possiblePos[UnityEngine.Random.Range(0, possiblePos.Count)];
        }
        _uITilemap.SetTile(_tileChosenPos, _allyTile);
        animator.SetTrigger("TileChosen");
        animator.SetBool("Attacking", true);
    }
    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
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

                if (InTile(vToW) == (int)CharType.EnemyCharacter || InTile(vToW) == (int)CharType.EnemyHero)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsHeroCovered()
    {
        //Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);

        //for (int i = 0; i < _heroFrontTiles.Count; i++)
        //{
        //    Vector3 vToW = _heroFrontTiles[i] + v;
        //    if (_cCB.InTile(vToW) == (int)CharType.Nothing)
        //        return false;
        //}
        //return true;
        return false;
    }
}