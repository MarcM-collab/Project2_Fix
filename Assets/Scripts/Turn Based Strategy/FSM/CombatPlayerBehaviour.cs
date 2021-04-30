using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatPlayerBehaviour : CombatBehaviour
{
    [SerializeField]
    protected GameObject EnemyHeroeGO;
    [SerializeField]
    protected CardUsage _cardUsage;

    protected static Vector3Int _currentGridPos;
    protected static Vector3Int _lastGridPos;
    protected static Vector3 _mousePos;
    protected static bool IsAttacking;

    protected static List<Vector3Int> _spawnableTilesEdges = new List<Vector3Int>();
    private void OnEnable()
    {

        
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileUpdate += Melee_ChoosingTileUpdate;
        RangedChoosingTileBehaviour.OnRangedChoosingTileUpdate += Ranged_ChoosingTileUpdate;
        MeleeShowAttackRangeBehaviour.OnMeleeShowAttackRangeEnter += Melee_ShowAttackRangeEnter;
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileUpdate += Melee_ChoosingAttackTileUpadate;
        MeleeHideAttackRangeBehaviour.OnMeleeHideAttackRangeEnter += Melee_HideAttackRangeEnter;
        HideRangeBehaviour.OnHideRangeEnter += HideRangeEnter;
    }

    private void OnDisable()
    {

        

        MeleeChoosingTileBehaviour.OnMeleeChoosingTileUpdate -= Melee_ChoosingTileUpdate;
        RangedChoosingTileBehaviour.OnRangedChoosingTileUpdate -= Ranged_ChoosingTileUpdate;
        HideRangeBehaviour.OnHideRangeEnter -= HideRangeEnter;
    }
    private void Start()
    {
        _enemyHero = EnemyHeroeGO.GetComponent<Hero>();
        _enemyHeroTiles = new List<Vector3Int> { new Vector3Int(6, 0, 0), new Vector3Int(7, 0, 0), new Vector3Int(6, -1, 0), new Vector3Int(7, -1, 0) };
        _enemyHeroAttackableTiles = new List<Vector3Int> { new Vector3Int(6, 0, 0), new Vector3Int(6, -1, 0) };
        _spawnableTilesEdges.Add(new Vector3Int(-6, -3, 0));
        _spawnableTilesEdges.Add(new Vector3Int(-5, 2, 0));
    }
    private void Update()
    {
        _mousePos = Input.mousePosition;
        _currentGridPos = _floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(_mousePos));
    }
    private void Ranged_ChoosingTileUpdate(Animator animator)
    {
        TileSetter();

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);
            else if (_targetTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenGridPosition = _executorGridPos;
                _targetGridPosition = _currentGridPos;
                EntityManager.SetTarget(SelectCharacter());
                EntityManager.SetTarget(_targetEntity);
                animator.SetBool("Attacking", true);
                IsAttacking = true;
                animator.SetTrigger("TileChosen");
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
                animator.SetTrigger("TileChosen");
            }
        }
        if (IsEnemy())
            Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
    //----------------------------------------Melee_ChoosingTile----------------------------------------
    private void Melee_ChoosingTileUpdate(Animator animator)
    {
        if (IsEnemy())
        {
            Cursor.SetCursor(_cursorSword, Vector2.zero, CursorMode.Auto);
        }
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);

        TileSetter();

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);

            else if (_uITilemap.GetTile(_currentGridPos) == _targetTile)
            {
                _targetGridPosition = _currentGridPos;
                EntityManager.SetTarget(SelectEntity());
                Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
                animator.SetBool("PreparingAttack", true);
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_currentGridPos, _allyTile);
                animator.SetTrigger("TileChosen");
            }
        }
    }
    private void TileSetter()
    {
        //range grid to outside
        if (_currentGridPos != _lastGridPos)
        {
            if (ToRange()) //only _pointing
            {
                _uITilemap.SetTile(_lastGridPos, _rangeTile);
                _lastGridPos = _currentGridPos;
            }
            //outside to range grid
            else if (ToPoint()) //only _range
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (ToCross()) //_range and pointing
            {
                _uITilemap.SetTile(_lastGridPos, _rangeTile);
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
        }
    }
    //----------------------------------------Melee_ShowAttackRange----------------------------------------
    private void Melee_ShowAttackRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        if (!(_targetEntity.GetComponent("Character") as Entity is null))
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = _targetGridPosition + vector;
                    Vector3 vToW = pos + v;

                    if (!(i == 0 && j == 0) && _uITilemap.HasTile(pos) && (pos == _executorGridPos || !(InTile(vToW) == (int)EntityType.AllyCharacter)) && !_collisionTilemap.HasTile(pos))
                    {
                        _uITilemap.SetTile(pos, _targetTile);
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < _enemyHeroAttackableTiles.Count; x++)
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = _enemyHeroAttackableTiles[x] + vector;
                    Vector3 vToW = pos + v;

                    if (!(i == 0 && j == 0) && _uITilemap.HasTile(pos) && (pos == _executorGridPos || !(InTile(vToW) == (int)EntityType.AllyCharacter)) && !_collisionTilemap.HasTile(pos))
                    {
                        _uITilemap.SetTile(pos, _targetTile);
                    }
                }
            }
        }
    }
    //----------------------------------------Melee_ChoosingAttackTile----------------------------------------
    private void Melee_ChoosingAttackTileUpadate(Animator animator)
    {
        TileAttackingSetter();

        if (InputManager.LeftMouseClick)
        {
            if (_pointingTile != _uITilemap.GetTile(_currentGridPos) || IsEnemy())
            {
                animator.SetBool("PreparingAttack", false);
            }

            else
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_currentGridPos, _allyTile);
                animator.SetTrigger("TileChosen");
                animator.SetBool("Attacking", true);
                IsAttacking = true;
            }
        }
    }
    private void TileAttackingSetter()
    {
        if (_currentGridPos != _lastGridPos)
        {
            if ((!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos) || _rangeTile == _uITilemap.GetTile(_currentGridPos) || _targetGridPosition == _currentGridPos || IsEnemy() || _enemyHeroTiles.Contains(_currentGridPos))
            && _uITilemap.HasTile(_lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos)) //only _pointing
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _rangeTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos) || _targetGridPosition == _lastGridPos || _enemyHeroTiles.Contains(_lastGridPos))
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos) && !IsEnemy() && !_enemyHeroTiles.Contains(_currentGridPos)) //only _range
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos) && _targetTile == _uITilemap.GetTile(_currentGridPos) && _targetGridPosition != _lastGridPos && _targetGridPosition != _currentGridPos && !_enemyHeroTiles.Contains(_lastGridPos) && !_enemyHeroTiles.Contains(_currentGridPos)
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _rangeTile != _uITilemap.GetTile(_lastGridPos)
            && _allyTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos)
            && _nullTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos) && !IsEnemy() && !_enemyHeroTiles.Contains(_currentGridPos)) //_range and pointing
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
        }
    }
    //----------------------------------------Melee_HideAttackRange----------------------------------------
    private void Melee_HideAttackRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int x = _uITilemap.cellBounds.min.x; x < _uITilemap.cellBounds.max.x; x++)
        {
            for (int y = _uITilemap.cellBounds.min.y; y < _uITilemap.cellBounds.max.y; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);
                var pos = _targetGridPosition + vector;
                Vector3 vToW = pos + v;

                if (_uITilemap.GetTile(pos) == _targetTile && InTile(vToW) == (int)EntityType.Nothing)
                    _uITilemap.SetTile(pos, _rangeTile);
            }
        }
    }
    //----------------------------------------HideRange----------------------------------------
    private void HideRangeEnter()
    {
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap, null, Hide1);
        IsAttacking = false;
    }
    private bool Hide1(Vector3Int vector)
    {
        var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var pos = vector + v;
        if (IsAttacking)
        {
            return (((_uITilemap.GetTile(vector) != _targetTile || (InTile(pos) != (int)EntityType.EnemyHero && InTile(pos) != (int)EntityType.EnemyCharacter)))
                && vector != _tileChosenGridPosition && vector != _executorGridPos);
        }
        else
        {
            return (vector != _tileChosenGridPosition && vector != _executorGridPos);
        }
    }

    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
    protected Character SelectCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                return gameObject.GetComponent<Character>();
            }
        }
        return null;
    }
    private static Entity SelectEntity()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
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
    private bool ToRange()
    {
        return (!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos) || _targetTile == _uITilemap.GetTile(_currentGridPos))
            && _uITilemap.HasTile(_lastGridPos)
            && _targetTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos);
    }
    private bool ToPoint()
    {
        return _uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _targetTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos))
            && _targetTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos);
    }
    private bool ToCross()
    {
        return _uITilemap.HasTile(_currentGridPos) && _rangeTile == _uITilemap.GetTile(_currentGridPos)
            && _targetTile != _uITilemap.GetTile(_currentGridPos) && _targetTile != _uITilemap.GetTile(_lastGridPos)
            && _allyTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos)
            && _nullTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos);
    }
    private static bool IsEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Entity is null))
            {
                if (EntityManager.ExecutorCharacter.Team != gameObject.GetComponent<Entity>().Team)
                    return true;
            }
        }
        return false;
    }
    protected bool CanMove(Vector3Int pos)
    {
        if (!_floorTilemap.HasTile(pos) || _collisionTilemap.HasTile(pos))
            return false;
        return true;
    }
}
