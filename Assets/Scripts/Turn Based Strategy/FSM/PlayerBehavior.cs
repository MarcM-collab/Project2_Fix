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
    private static int _couterPointing;

    private static Entity _selectedEntity;
    private static Vector3Int _selectedGridPos;
    private static Vector3 _mousePos;
    private static Vector3Int _tileChosenPos;

    private bool _isLastTargetTile;

    private static bool _isExecutorSelected;
    private void OnEnable()
    {
        Selecting.OnSelectingUpdate += SelectingUpdate;
        Melee_ShowRange.OnMelee_ShowRangeEnter += Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter += Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate += Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate += Ranged_ChoosingTileUpdate;
        HideRange.OnHideRangeEnter += HideRangeEnter;
        MovingToTile.OnMovingToTileEnter += MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate += MovingToTileUpdate;
        HideUI.OnHideUIEnter += HideUIEnter;
        Attacking.OnAttackingEnter += AttackingEnter;
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
        Attacking.OnAttackingEnter -= AttackingEnter;
        HideUI.OnHideUIEnter -= HideUIEnter;
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;
        _currentGridPos = _floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(_mousePos));
    }

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
            _selectedEntity = SelectEntity();
            if (!(_selectedEntity is null))
            {
                if (EntityManager.IsEntityInList(EntityManager.GetActiveAllies(_selectedEntity), _selectedEntity))
                {
                    Debug.Log(_selectedEntity.name);
                    EntityManager.SetExecutor(_selectedEntity);
                    _selectedGridPos = _currentGridPos;
                    _isExecutorSelected = true;
                    if (_selectedEntity.Class == Class.Ranged)
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
    private void Melee_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _selectedEntity.Range + 1;
        int counter = 0;

        for (int j = -range; j <= range; j++)
        {
            if (j <= 0) counter++;
            else counter--;

            for (int i = -range; i <= range; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = _selectedGridPos + vector;
                Vector3 vToW = pos + v;

                if (Mathf.Abs(i) < counter - 1)
                {
                    if (InTile(vToW) == 1)
                        _uITilemap.SetTile(pos, _targetTile);
                    else if (CanMove(pos) && InTile(vToW) == 0)
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (pos == _selectedGridPos || InTile(vToW) == 2)
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
    private void Ranged_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _selectedEntity.Range;
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
                    var pos = _selectedGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (pos == _selectedGridPos || InTile(vToW) == 2)
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
    }
    private static int InTile(Vector3 vector)
    {
        RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                if (_selectedEntity.Team != gameObject.GetComponent<Entity>().Team)
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
    private void Ranged_ChoosingTileUpdate(Animator animator)
    {
        TileSetter();

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos))
                animator.SetBool("Selected", false);
            else
            {
                _tileChosenPos = _currentGridPos;
                animator.SetTrigger("TileChosen");
            }
        }
        if (IsEnemy())
            Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
    private void Melee_ChoosingTileUpdate(Animator animator)
    {
        if (_isLastTargetTile && _currentGridPos != _lastGridPos)
        {
            _couterPointing++;
            if (_couterPointing == 1)
            {
                _couterPointing = 0;
                DeletePointingTile();
                _isLastTargetTile = false;
            }
        }

        TileSetter();

        if (_targetTile == _uITilemap.GetTile(_lastGridPos))
        {
            _isLastTargetTile = true;
        }

        if (InputManager.LeftMouseClick)
        {
            if (!_uITilemap.HasTile(_currentGridPos))
                animator.SetBool("Selected", false);
            else
            {
                _tileChosenPos = _currentGridPos;
                animator.SetTrigger("TileChosen");
            }
        }
        if (IsEnemy())
        {
            Cursor.SetCursor(_cursorSword, Vector2.zero, CursorMode.Auto);
        }
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
    private void TileSetter()
    {
        //range grid to outside
        if (ToPoint())
        {
            _uITilemap.SetTile(_lastGridPos, _rangeTile);
        }
        //outside to range grid
        else if (ToRange())
        {
            _uITilemap.SetTile(_currentGridPos, _pointingTile);
        }
        else if (ToCross())
        {
            _uITilemap.SetTile(_lastGridPos, _rangeTile);
            _uITilemap.SetTile(_currentGridPos, _pointingTile);
        }
        _lastGridPos = _currentGridPos;
    }
    private bool ToPoint()
    {
        return (!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos))
            && _uITilemap.HasTile(_lastGridPos)
            && _targetTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos);
    }
    private bool ToRange()
    {
        return _uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _targetTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos))
            && _targetTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos);
    }
    private bool ToCross()
    {
        return _uITilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos && _rangeTile == _uITilemap.GetTile(_currentGridPos)
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
    private void DeletePointingTile()
    {
        for (int x = _floorTilemap.cellBounds.min.x; x < _floorTilemap.cellBounds.max.x; x++)
        {
            for (int y = _floorTilemap.cellBounds.min.y; y < _floorTilemap.cellBounds.max.y; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);

                if (_pointingTile == _uITilemap.GetTile(vector))
                    _uITilemap.SetTile(vector, _rangeTile);
            }
        }
    }
    private void MovingToTileEnter()
    {
        //PathFinding
        List<Vector3> list = new List<Vector3>();
        Vector3 v = new Vector3(_floorTilemap.cellSize.x/2, _floorTilemap.cellSize.y / 2);
        list.Add(_selectedGridPos + v);
        list.Add(_tileChosenPos + v);
        EntityManager.ExecutorEntity.Path = list;

        EntityManager.ExecutorEntity.Walking = true;
    }
    private void MovingToTileUpdate(Animator animator)
    {
        animator.SetBool("MovedToTile", !EntityManager.ExecutorEntity.Walking);
    }
    private void HideRangeEnter()
    {
    }

    private void HideUIEnter()
    {
    }

    private void AttackingEnter()
    {
    }
}
