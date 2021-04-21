using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Tile _pointingTile;
    [SerializeField]
    private Tile _rangeTile;
    [SerializeField]
    private Tile _targetTile;
    [SerializeField]
    private Tile _nullTile;
    [SerializeField]
    private Tile _allyTile;


    [SerializeField]
    private Camera _camera;
    [SerializeField]
    public Tilemap _floorTilemap;
    [SerializeField]
    public Tilemap _collisionTilemap;
    [SerializeField]
    public Tilemap _uITilemap;

    [SerializeField]
    private Texture2D _cursorHand;
    [SerializeField]
    private Texture2D _cursorSword;
    [SerializeField]
    private Texture2D _cursorArrow;

    private static Vector3Int _currentGridPos;
    private static Vector3Int _lastGridPos;

    private static Entity _executorEntity;
    private static Entity _targetEntity;
    private static Vector3Int _executorGridPos;
    private static Vector3 _mousePos;
    private static Vector3Int _tileChosenPos;
    private static Vector3Int _targetGridPos;


    private bool _isLastTargetTile;

    private static bool _isExecutorSelected;
    private void OnEnable()
    {
        Selecting.OnSelectingUpdate += SelectingUpdate;
        Melee_ShowRange.OnMelee_ShowRangeEnter += Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter += Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate += Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate += Ranged_ChoosingTileUpdate;
        Melee_ShowAttackRange.OnMelee_ShowAttackRangeEnter += Melee_ShowAttackRangeEnter;
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileUpdate += Melee_ChoosingAttackTileUpadate;
        Melee_HideAttackRange.OnMelee_HideAttackRangeEnter += Melee_HideAttackRangeEnter;
        HideRange.OnHideRangeEnter += HideRangeEnter;
        MovingToTile.OnMovingToTileEnter += MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate += MovingToTileUpdate;
        HidePath.OnHidePathEnter += HidePathEnter;
        Attacking.OnAttackingEnter += AttackingEnter;
        Attacking.OnAttackingUpdate += AttackingUpdate;
        Attacking.OnAttackingExit += AttackingExit;
        HideUI.OnHideUIEnter += HideUIEnter;
        ExhaustingAndReset.OnExhaustingAndResetEnter += ExhaustingAndResetEnter;
    }
    private void OnDisable()
    {
        Selecting.OnSelectingUpdate -= SelectingUpdate;
        Melee_ShowRange.OnMelee_ShowRangeEnter -= Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter -= Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate -= Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate -= Ranged_ChoosingTileUpdate;
        HideRange.OnHideRangeEnter -= HideRangeEnter;
        MovingToTile.OnMovingToTileEnter -= MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate -= MovingToTileUpdate;
        HidePath.OnHidePathEnter -= HidePathEnter;
        Attacking.OnAttackingEnter -= AttackingEnter;
        Attacking.OnAttackingUpdate += AttackingUpdate;
        Attacking.OnAttackingExit += AttackingExit;
        HideUI.OnHideUIEnter -= HideUIEnter;
        ExhaustingAndReset.OnExhaustingAndResetEnter -= ExhaustingAndResetEnter;
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;
        _currentGridPos = _floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(_mousePos));
    }

    //----------------------------------------Selecting----------------------------------------
    private void SelectingUpdate(Animator animator)
    {
        if (!_isExecutorSelected && _floorTilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos)
        {
            _uITilemap.SetTile(_lastGridPos, null);
            _uITilemap.SetTile(_currentGridPos, _pointingTile);
            _lastGridPos = _currentGridPos;
        }

        if (InputManager.LeftMouseClick)
        {
            _executorEntity = SelectEntity();
            if (!(_executorEntity is null))
            {
                if (EntityManager.IsEntityInList(EntityManager.GetActiveAllies(_executorEntity), _executorEntity))
                {
                    EntityManager.SetExecutor(_executorEntity);
                    _executorGridPos = _currentGridPos;
                    _isExecutorSelected = true;
                    if (_executorEntity.Class == Class.Ranged)
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
    //----------------------------------------Melee_ShowRange----------------------------------------
    private void Melee_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorEntity.Range + 1;
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

                if (Mathf.Abs(i) < counter - 1)
                {
                    if (InTile(vToW) == 1)
                        _uITilemap.SetTile(pos, _targetTile);
                    else if (CanMove(pos) && InTile(vToW) == 0)
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (pos == _executorGridPos || InTile(vToW) == 2)
                        _uITilemap.SetTile(pos, _allyTile);
                    else if (_floorTilemap.HasTile(pos))
                        _uITilemap.SetTile(pos, _nullTile);
                }
                else
                {
                    if (InTile(vToW) == 1)
                        _uITilemap.SetTile(pos, _targetTile);
                }
            }
        }
    }
    //----------------------------------------Ranged_ShowRange----------------------------------------
    private void Ranged_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorEntity.Range;
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
                //var pos = _selectedGridPos + vector;
                //Vector3 vToW = pos + v;
                Vector3 vToW = vector + v;
                if (InTile(vToW) == 1)
                    _uITilemap.SetTile(vector, _targetTile);
            }
        }
    }    //----------------------------------------HidePathEnter----------------------------------------
    private static int InTile(Vector3 vector)
    {
        RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                if (_executorEntity.Team != gameObject.GetComponent<Entity>().Team)
                    return 1; //enemy
                else
                    return 2; //ally
            }
        }
        return 0; //nothing
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
                _targetEntity = SelectEntity();
                EntityManager.SetTarget(_targetEntity);
                animator.SetBool("Attacking", true);
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
        //if (_isLastTargetTile && _currentGridPos != _lastGridPos)
        //{
        //    _couterPointing++;
        //    if (_couterPointing == 1)
        //    {
        //        _couterPointing = 0;
        //        DeletePointingTile();
        //        _isLastTargetTile = false;
        //    }
        //}

        TileSetter();

        //if (_targetTile == _uITilemap.GetTile(_lastGridPos))
        //{
        //    _isLastTargetTile = true;
        //}

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);
            else if (_targetTile == _uITilemap.GetTile(_currentGridPos))
            {
                _targetGridPos = _currentGridPos;
                _targetEntity = SelectEntity();
                EntityManager.SetTarget(_targetEntity);
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

        if (IsEnemy() && _targetEntity is null)
        {
            Cursor.SetCursor(_cursorSword, Vector2.zero, CursorMode.Auto);
        }
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
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
    //private void DeletePointingTile()
    //{
    //    for (int x = _floorTilemap.cellBounds.min.x; x < _floorTilemap.cellBounds.max.x; x++)
    //    {
    //        for (int y = _floorTilemap.cellBounds.min.y; y < _floorTilemap.cellBounds.max.y; y++)
    //        {
    //            Vector3Int vector = new Vector3Int(x, y, 0);

    //            if (_pointingTile == _uITilemap.GetTile(vector))
    //                _uITilemap.SetTile(vector, _rangeTile);
    //        }
    //    }
    //}
    //----------------------------------------Melee_ShowAttackRange----------------------------------------
    private void Melee_ShowAttackRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _targetGridPos + vector;
                Vector3 vToW = pos + v;

                if (!(i == 0 && j == 0) && _uITilemap.HasTile(pos) && !(InTile(vToW) == 2) && !_collisionTilemap.HasTile(pos))
                {
                    _uITilemap.SetTile(pos, _targetTile);
                }
            }
        }
        //Then remove range
    }

    //----------------------------------------Melee_ChoosingAttackTile----------------------------------------
    private void Melee_ChoosingAttackTileUpadate(Animator animator)
    {
        TileAttackingSetter();

        if (InputManager.LeftMouseClick)
        {
            if (!_targetTile == _uITilemap.GetTile(_currentGridPos) || IsEnemy())
            {
                animator.SetBool("PreparingAttack", false);
                _targetEntity = null;
            }

            else
            {
                _tileChosenPos = _currentGridPos;
                _uITilemap.SetTile(_currentGridPos, _allyTile);
                animator.SetTrigger("TileChosen");
                animator.SetBool("Attacking", true);
            }
        }
    }
    private void TileAttackingSetter()
    {
        //range grid to outside
        if (_currentGridPos != _lastGridPos)
        {
            if ((!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos) || _rangeTile == _uITilemap.GetTile(_currentGridPos) || _targetGridPos == _currentGridPos)
            && _uITilemap.HasTile(_lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos)) //only _pointing
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
                _lastGridPos = _currentGridPos;
            }
            //outside to range grid
            else if (_uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _rangeTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos) || _targetGridPos == _lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos) && !IsEnemy()) //only _range
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos) && _targetTile == _uITilemap.GetTile(_currentGridPos) && _targetGridPos != _lastGridPos && _targetGridPos != _currentGridPos
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _rangeTile != _uITilemap.GetTile(_lastGridPos)
            && _allyTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos)
            && _nullTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos)) //_range and pointing
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
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _targetGridPos + vector;
                Vector3 vToW = pos + v;

                if (!(i == 0 && j == 0) && _uITilemap.HasTile(pos) && !(InTile(vToW) == 2) && !_collisionTilemap.HasTile(pos))
                {
                    _uITilemap.SetTile(pos, _rangeTile);
                }
            }
        }
    }
    //----------------------------------------HideRange----------------------------------------
    private void HideRangeEnter()
    {
        HideTilemapElements(_uITilemap, Hide1);
    }
    private bool Hide1(Vector3 vector)
    {
        return !(_executorGridPos == vector || _tileChosenPos == vector || _targetGridPos == vector);
    }
    //----------------------------------------MovingToTileEnter----------------------------------------
    private void MovingToTileEnter()
    {
        //PathFinding
        List<Vector3> list = new List<Vector3>();
        Vector3 v = new Vector3(_floorTilemap.cellSize.x/2, _floorTilemap.cellSize.y / 2);
        list.Add(_executorGridPos + v);
        list.Add(_tileChosenPos + v);
        _executorEntity.Path = list;

        var deltaX = list[list.Count - 1].x - list[0].x;

        TurningExecutor(deltaX);
        
        _executorEntity.Walking = true;
    }
    private void MovingToTileUpdate(Animator animator)
    {
        animator.SetBool("MovedToTile", !_executorEntity.Walking);
    }
    //----------------------------------------HidePath----------------------------------------
    private void HidePathEnter()
    {
        HideTilemapElements(_uITilemap, Hide2);
    }
    private bool Hide2 (Vector3 vector)
    {
        return !(_targetGridPos == vector);
    }
    //----------------------------------------Attacking----------------------------------------
    private void AttackingEnter()
    {
        var deltaX = _targetGridPos.x - _tileChosenPos.x;
        TurningExecutor(deltaX);
    }
    private void AttackingUpdate(Animator animator)
    {
        animator.SetBool("Attacking", _executorEntity.Turn);
    }
    private void AttackingExit()
    {
        _executorEntity.Attack();
    }
    //----------------------------------------HideUI----------------------------------------
    private void HideUIEnter()
    {
        HideTilemapElements(_uITilemap, Hide3);
    }
    private bool Hide3(Vector3 vector)
    {
        return true;
    }
    //----------------------------------------ExhaustingAndReset----------------------------------------
    private void ExhaustingAndResetEnter(Animator animator)
    {
        _executorEntity.Exhausted = true;
        animator.SetBool("Selected", false);
        animator.SetBool("TileChosen", false);
        animator.SetBool("MovedToTile", false);
        animator.SetBool("Ranged", false);
        animator.SetBool("Attacking", false);
        animator.SetBool("PreparingAttack", false);
        Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }

    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
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
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                if (EntityManager.ExecutorEntity.Team != gameObject.GetComponent<Entity>().Team)
                    return true;
            }
        }
        return false;
    }
    private void HideTilemapElements(Tilemap tilemap, System.Func<Vector3, bool> function)
    {
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);

                if (function(vector))
                    _uITilemap.SetTile(vector, null);
            }
        }
    }
    private void TurningExecutor(float deltaX)
    {
        if (deltaX < 0)
        {
            Debug.Log(_executorEntity.transform.rotation.eulerAngles.y);
            if (_executorEntity.transform.rotation.eulerAngles.y == 180)
            {
                _executorEntity.Turn = true;
            }
        }
        else
        {
            if (_executorEntity.transform.rotation.eulerAngles.y == 0)
            {
                _executorEntity.Turn = true;
            }
        }
    }
}
