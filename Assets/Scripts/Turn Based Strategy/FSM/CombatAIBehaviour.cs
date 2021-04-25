using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : MonoBehaviour
{

    public CombatCommonBehaviour _cCB;
    public Heroe _heroe;

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

        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
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
                    if (_cCB.InTile(vToW) == (int)CharType.Enemy && i != -range && i != range && j != -range && j != range)
                    {
                        if (minPos.x + minPos.y > i + j)
                        {
                            minPos = new Vector3Int(Mathf.Abs(i), Mathf.Abs(j), 0);
                            CombatCommonBehaviour.TargetGridPos = pos;
                            targetOnRange = true;
                        }
                    }
                }
            }
        }
        if (targetOnRange)
        {
            Vector2 vector2 = new Vector2(CombatCommonBehaviour.TargetGridPos.x, CombatCommonBehaviour.TargetGridPos.y);
            RaycastHit2D hit = Physics2D.Raycast(vector2, Vector2.zero);
            var hitCollider = hit.collider;
            if (hitCollider != null)
            {
                var gameObject = hitCollider.gameObject;
                if (!(gameObject.GetComponent("Character") as Character is null))
                {
                    CharacterManager.SetTarget(gameObject.GetComponent<Character>());
                    Debug.Log(gameObject.GetComponent<Character>().name);
                }
            }

            _uITilemap.SetTile(CombatCommonBehaviour.TargetGridPos, _targetTile);
            animator.SetBool("PreparingAttack", true);
        }
        else
        {
            range = _executorCharacter.Range;
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
                        if (!(_cCB.InTile(vToW) == (int)CharType.Enemy || _cCB.InTile(vToW) == (int)CharType.Ally || _collisionTilemap.HasTile(pos) || !_floorTilemap.HasTile(pos)))
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
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = CombatCommonBehaviour.TargetGridPos + vector;
                Vector3 vToW = pos + v;

                if (!(i == 0 && j == 0) && _uITilemap.HasTile(pos) && !(_cCB.InTile(vToW) == (int)CharType.Ally) && !_collisionTilemap.HasTile(pos))
                {
                    _uITilemap.SetTile(pos, _targetTile);
                }
            }
        }
        animator.SetBool("Attacking", true);
    }
    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
}