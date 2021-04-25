using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : MonoBehaviour
{

    public CombatCommonBehaviour _cCB;
    public Hero _heroe;
    private List<Vector3Int> _heroTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-8, 0, 0), new Vector3Int(-7, -1, 0), new Vector3Int(-8, -1, 0) };
    private List<Vector3Int> _heroAttackableTiles = new List<Vector3Int> { new Vector3Int(-7, 0, 0), new Vector3Int(-7, -1, 0) };
    private List<Vector3Int> _heroFrontTiles = new List<Vector3Int> { new Vector3Int(-6, 0, 0), new Vector3Int(-6, -1, 0)};

    private Tile _pointingTile => _cCB.PointingTile;
    private Tile _rangeTile => _cCB.RangeTile;
    private Tile _targetTile => _cCB.TargetTile;
    private Tile _nullTile => _cCB.NullTile;
    private Tile _allyTile => _cCB.AllyTile;

    private Tilemap _floorTilemap => _cCB.FloorTilemap;
    private Tilemap _collisionTilemap => _cCB.CollisionTilemap;
    private Tilemap _uITilemap => _cCB.UITilemap;

    private static Character _executorCharacter => CombatCommonBehaviour.ExecutorCharacter;
    private static Character _targetCharacter => CombatCommonBehaviour.TargetCharacter;

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
    //----------------------------------------Selecting----------------------------------------
    private void SelectingEnter(Animator animator)
    {
        var characters = CharacterManager.GetActiveCharacters(Team.TeamAI);
        if (characters.Length == 0)
        {
            CharacterManager.RemoveExhaust();
            characters = CharacterManager.GetActiveCharacters(Team.TeamAI);
        }
        CharacterManager.SetExecutor(characters[0]);

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

        CombatCommonBehaviour.ExecutorGridPos = _currentGridPos;
        _uITilemap.SetTile(CombatCommonBehaviour.ExecutorGridPos, _allyTile);

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
                var pos = CombatCommonBehaviour.ExecutorGridPos + vector;
                Vector3 vToW = pos + v;

                if (Mathf.Abs(i) < counter)
                {
                    if (i != -range && i != range && j != -range && j != range) 
                    {
                        if (!IsHeroCovered()) 
                        {
                            if (_cCB.InTile(vToW) == (int)CharType.EnemyHero && _heroAttackableTiles.Contains(pos))
                            {
                                CombatCommonBehaviour.TargetGridPos = pos;
                                targetOnRange = true;
                                break;
                            }
                        }
                        else if (_cCB.InTile(vToW) == (int)CharType.EnemyCharacter)
                        {
                            if (minPos.x + minPos.y > Mathf.Abs(i) + Mathf.Abs(j))
                            {
                                minPos = new Vector3Int(Mathf.Abs(i), Mathf.Abs(j), 0);
                                CombatCommonBehaviour.TargetGridPos = pos;
                                targetOnRange = true;
                            }
                        }
                    }
                }
            }

        }
        if (targetOnRange)
        {
            Vector2 vector2 = new Vector2(CombatCommonBehaviour.TargetGridPos.x + v.x, CombatCommonBehaviour.TargetGridPos.y + v.y);
            RaycastHit2D hit = Physics2D.Raycast(vector2, Vector2.zero);
            var hitCollider = hit.collider;
            if (hitCollider != null)
            {
                if (!(hitCollider.gameObject.GetComponent("Character") as Character is null))
                {
                    CharacterManager.SetTarget(hitCollider.gameObject.GetComponent<Character>());
                    _uITilemap.SetTile(CombatCommonBehaviour.TargetGridPos, _targetTile);
                }
                if (!(hitCollider.gameObject.GetComponent("Hero") as Hero is null))
                {
                    for (int i = 0; i < _heroTiles.Count; i++)
                    {
                        _uITilemap.SetTile(_heroTiles[i], _targetTile);
                    }
                }
                animator.SetBool("PreparingAttack", true);
            }
        }
        else
        {
            range = _executorCharacter.Range;
            counter = 0;
            var min = Vector3.Distance(CombatCommonBehaviour.ExecutorGridPos, _heroe.transform.position);

            for (int j = -range; j <= range; j++)
            {
                if (j <= 0) counter++;
                else counter--;

                for (int i = -range; i <= range; i++)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = CombatCommonBehaviour.ExecutorGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (Mathf.Abs(i) < counter - 1)
                    {
                        if (!(_cCB.InTile(vToW) == (int)CharType.EnemyCharacter || _cCB.InTile(vToW) == (int)CharType.AllyCharacter || _collisionTilemap.HasTile(pos) || !_floorTilemap.HasTile(pos)))
                        {
                            var currentMin = Vector3.Distance(vToW, _heroe.transform.position);
                            if (min > currentMin)
                            {
                                min = currentMin;
                                CombatCommonBehaviour.TileChosenPos = pos;
                            }
                        }
                    }
                }
            }
            _uITilemap.SetTile(CombatCommonBehaviour.TileChosenPos, _allyTile);
            animator.SetTrigger("TileChosen");
        }
    }
    
    //----------------------------------------Ranged_ChoosingTile----------------------------------------
    private void Ranged_ChoosingTileEnter(Animator animator)
    {
        var characters = CharacterManager.GetActiveCharacters(Team.TeamPlayer);

        CharacterManager.SetTarget(characters[0]);
        CombatCommonBehaviour.TargetGridPos = _floorTilemap.WorldToCell(characters[0].transform.position);
        _uITilemap.SetTile(CombatCommonBehaviour.TargetGridPos, _targetTile);

        CombatCommonBehaviour.TileChosenPos = CombatCommonBehaviour.ExecutorGridPos;

        animator.SetBool("Attacking", true);
        animator.SetTrigger("TileChosen");
    }
    //----------------------------------------Melee_ChoosingAttackTile----------------------------------------
    private void Melee_ChoosingAttackTileEnter(Animator animator)
    {
        if (IsEnemyNeighbour())
        {
            CombatCommonBehaviour.TileChosenPos = CombatCommonBehaviour.ExecutorGridPos;
        }
        else
        {
            var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = CombatCommonBehaviour.TargetGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (!(i == 0 && j == 0) && _floorTilemap.HasTile(pos) && !_collisionTilemap.HasTile(pos) && _cCB.InTile(vToW) == (int)CharType.Nothing)
                    {
                        CombatCommonBehaviour.TileChosenPos = pos;
                    }
                }
            }
        }
        _uITilemap.SetTile(CombatCommonBehaviour.TileChosenPos, _allyTile);
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
                var pos = CombatCommonBehaviour.ExecutorGridPos + vector;
                Vector3 vToW = pos + v;

                if (_cCB.InTile(vToW) == (int)CharType.EnemyCharacter && _cCB.InTile(vToW) == (int)CharType.EnemyHero)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsHeroCovered()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);

        for (int i = 0; i < _heroFrontTiles.Count; i++)
        {
            Vector3 vToW = _heroFrontTiles[i] + v;
            if (_cCB.InTile(vToW) == (int)CharType.Nothing)
                return false;
        }
        return true;
    }
}