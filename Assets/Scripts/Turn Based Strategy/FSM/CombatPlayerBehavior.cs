using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatPlayerBehavior : CombatBehaviour
{
    public GameObject EnemyHeroeGO;

    private static Vector3Int _currentGridPos;
    private static Vector3Int _lastGridPos;
    private static Vector3 _mousePos;
    private static bool IsAttacking;

    private static List<Vector3Int> _spawnableTilesEdges = new List<Vector3Int>();

    public CardUsage _cardUsage;
    private void OnEnable()
    {
        Spawning.OnSpawningEnter += SpawningEnter;
        Spawning.OnSpawningUpdate += SpawningUpdate;
        Spawning.OnSpawningExit += SpawningExit;
        Selecting.OnSelectingUpdate += SelectingUpdate;
        Selecting.OnSelectingExit += SelectingExit;
        Melee_ShowRange.OnMelee_ShowRangeEnter += Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter += Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate += Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate += Ranged_ChoosingTileUpdate;
        Melee_ShowAttackRange.OnMelee_ShowAttackRangeEnter += Melee_ShowAttackRangeEnter;
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileUpdate += Melee_ChoosingAttackTileUpadate;
        Melee_HideAttackRange.OnMelee_HideAttackRangeEnter += Melee_HideAttackRangeEnter;
        HideRange.OnHideRangeEnter += HideRangeEnter;
    }

    private void OnDisable()
    {
        Spawning.OnSpawningEnter -= SpawningEnter;
        Spawning.OnSpawningUpdate -= SpawningUpdate;
        Spawning.OnSpawningExit -= SpawningExit;
        Selecting.OnSelectingUpdate -= SelectingUpdate;
        Selecting.OnSelectingExit -= SelectingExit;
        Melee_ShowRange.OnMelee_ShowRangeEnter -= Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter -= Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate -= Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate -= Ranged_ChoosingTileUpdate;
        HideRange.OnHideRangeEnter -= HideRangeEnter;
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

    //----------------------------------------Spawning----------------------------------------
    private void SpawningEnter()
    {
        SpawnableTiles(_allyTile);
    }
    private void SpawningUpdate(Animator animator)
    {
        if (animator.GetBool("IsDragging"))
        {
            if (_currentGridPos != _lastGridPos)
            {
                if (_uITilemap.GetTile(_currentGridPos) == _allyTile)
                {
                    _uITilemap.SetTile(_currentGridPos, _pointingTile);
                }
                else
                {
                    if (_uITilemap.GetTile(_lastGridPos) == _pointingTile)
                    {
                        _uITilemap.SetTile(_lastGridPos, _allyTile);
                    }
                }
                if (_uITilemap.GetTile(_lastGridPos) == _pointingTile)
                {
                    _uITilemap.SetTile(_lastGridPos, _allyTile);
                }
                _lastGridPos = _currentGridPos;
            }
        }

        if (InputManager.LeftMouseClick)
        { 
            if (_uITilemap.GetTile(_currentGridPos) == _pointingTile)
            {
                _cardUsage.Spawn();
            }
            animator.SetBool("IsDragging", false);
        }
    }
    private void SpawningExit()
    {
        HideSpawnableTiles();
    }

    //----------------------------------------Selecting----------------------------------------
    private void SelectingUpdate(Animator animator)
    {
        if (!animator.GetBool("IsDragging") && !animator.GetBool("Selected") && _floorTilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos)
        {
            _uITilemap.SetTile(_lastGridPos, null);
            _uITilemap.SetTile(_currentGridPos, _pointingTile);
            _lastGridPos = _currentGridPos;
        }
        else if (!_floorTilemap.HasTile(_currentGridPos))
        {
            _uITilemap.SetTile(_lastGridPos, null);
        }

        if (InputManager.LeftMouseClick)
        {
            var tempCharacter = SelectCharacter();
            if (!(tempCharacter is null))
            {
                if (EntityManager.IsEntityInList(EntityManager.GetActiveCharacters(Team.TeamPlayer), tempCharacter))
                {
                    EntityManager.SetExecutor(tempCharacter);
                    _executorGridPos = _currentGridPos;
                    if (tempCharacter.Class == Class.Ranged)
                    {
                        //Check if an enemy is in range
                        animator.SetBool("Ranged", true);
                    }
                    else
                    {
                        animator.SetBool("Ranged", false);
                    }
                    animator.SetBool("Selected", true);
                }
            }
        }
    }
    private void SelectingExit()
    {
    }
    //----------------------------------------Melee_ShowRange----------------------------------------
    private void Melee_ShowRangeEnter()
    {
        bool enemyHeroOnRange = false;

        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorCharacter.Range + 2;
        int counter = 0;

        for (int j = -range; j <= range; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -range; i <= range; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _executorGridPos + vector;
                Vector3 vToW = pos + v;

                if (Mathf.Abs(i) < counter - 2)
                {
                    if (InTile(vToW) == (int)EntityType.EnemyCharacter)
                        _uITilemap.SetTile(pos, _targetTile);
                    else if (CanMove(pos) && InTile(vToW) == (int)EntityType.Nothing)
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (pos == _executorGridPos || InTile(vToW) == (int)EntityType.AllyCharacter)
                        _uITilemap.SetTile(pos, _allyTile);
                    else if (_floorTilemap.HasTile(pos))
                        _uITilemap.SetTile(pos, _nullTile);
                    else if (InTile(vToW) == (int)EntityType.EnemyHero)
                        enemyHeroOnRange = true;
                }
                else if (Mathf.Abs(i) < counter)
                {
                    if (InTile(vToW) == (int)EntityType.EnemyCharacter && i != -range && i != range && j != -range && j != range)
                        _uITilemap.SetTile(pos, _targetTile);
                }
            }
        }
        if (enemyHeroOnRange)
        {
            ShowHeroTiles();
        }
    }
    //----------------------------------------Ranged_ShowRange----------------------------------------
    private void Ranged_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorCharacter.Range;
        int counter = 0;

        for (int j = -range; j <= range; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -range; i <= range; i++)
            {
                if (Mathf.Abs(i) < counter)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = _executorGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (pos == _executorGridPos || InTile(vToW) == 2)
                        _uITilemap.SetTile(pos, _allyTile);
                    else if (CanMove(pos))
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (_floorTilemap.HasTile(pos))
                        _uITilemap.SetTile(pos, _nullTile);
                }
            }
        }
        for (int x = _floorTilemap.cellBounds.min.x; x < _floorTilemap.cellBounds.max.x; x++)
        {
            for (int y = _floorTilemap.cellBounds.min.y; y < _floorTilemap.cellBounds.max.y; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);
                Vector3 vToW = vector + v;
                if (InTile(vToW) == 1)
                    _uITilemap.SetTile(vector, _targetTile);
            }
        }
    }
    private bool CanMove(Vector3Int pos)
    {
        if (!_floorTilemap.HasTile(pos) || _collisionTilemap.HasTile(pos))
            return false;
        return true;
    }
    //----------------------------------------Ranged_ChoosingTile----------------------------------------
    private void Ranged_ChoosingTileUpdate(Animator animator)
    {
        TileSetter();

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);
            else if (_targetTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenPos = _executorGridPos;
                _targetGridPos = _currentGridPos;
                EntityManager.SetTarget(SelectCharacter());
                EntityManager.SetTarget(_targetEntity);
                animator.SetBool("Attacking", true);
                IsAttacking = true;
                animator.SetTrigger("TileChosen");
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenPos = _currentGridPos;
                _uITilemap.SetTile(_tileChosenPos, _allyTile);
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
                _targetGridPos = _currentGridPos;
                EntityManager.SetTarget(SelectEntity());
                Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
                animator.SetBool("PreparingAttack", true);
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                _tileChosenPos = _currentGridPos;
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
                    var pos = _targetGridPos + vector;
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
                _tileChosenPos = _currentGridPos;
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
            if ((!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos) || _rangeTile == _uITilemap.GetTile(_currentGridPos) || _targetGridPos == _currentGridPos || IsEnemy() || _enemyHeroTiles.Contains(_currentGridPos))
            && _uITilemap.HasTile(_lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos)) //only _pointing
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _rangeTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos) || _targetGridPos == _lastGridPos || _enemyHeroTiles.Contains(_lastGridPos))
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos) && !IsEnemy() && !_enemyHeroTiles.Contains(_currentGridPos)) //only _range
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos) && _targetTile == _uITilemap.GetTile(_currentGridPos) && _targetGridPos != _lastGridPos && _targetGridPos != _currentGridPos && !_enemyHeroTiles.Contains(_lastGridPos) && !_enemyHeroTiles.Contains(_currentGridPos)
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
                var pos = _targetGridPos + vector;
                Vector3 vToW = pos + v;

                if (_uITilemap.GetTile(pos) == _targetTile && InTile(vToW) == (int)EntityType.Nothing)
                    _uITilemap.SetTile(pos, _rangeTile);
            }
        }
    }
    //----------------------------------------HideRange----------------------------------------
    private void HideRangeEnter()
    {
        HideTilemapElements(_uITilemap, Hide1);
        IsAttacking = false;
    }
    private bool Hide1(Vector3Int vector)
    {
        var v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var pos = vector + v;
        if (IsAttacking)
        {
            return (((_uITilemap.GetTile(vector) != _targetTile || (InTile(pos) != (int)EntityType.EnemyHero && InTile(pos) != (int)EntityType.EnemyCharacter)))
                && vector != _tileChosenPos && vector != _executorGridPos);
        }
        else
        {
            return (vector != _tileChosenPos && vector != _executorGridPos);
        }
    }

    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
    private static Character SelectCharacter()
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
    private void SpawnableTiles(Tile tile)
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int i = _spawnableTilesEdges[0].x; i <= _spawnableTilesEdges[1].x; i++)
        {
            for (int j = _spawnableTilesEdges[0].y; j <= _spawnableTilesEdges[1].y; j++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                Vector3 vToW = vector + v;
                if (InTile(vToW) == (int)EntityType.Nothing)
                    _uITilemap.SetTile(vector, tile);
            }
        }
    }
    private void HideSpawnableTiles()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int i = _spawnableTilesEdges[0].x; i <= _spawnableTilesEdges[1].x; i++)
        {
            for (int j = _spawnableTilesEdges[0].y; j <= _spawnableTilesEdges[1].y; j++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                Vector3 vToW = vector + v;
                    _uITilemap.SetTile(vector, null);
            }
        }
    }
}
