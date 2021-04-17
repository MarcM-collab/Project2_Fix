using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    public Tile _pointingTile;
    [SerializeField]
    public Tile _rangeTile;
    [SerializeField]
    public Tile _targetTile;
    [SerializeField]
    public Tile _nullTile;


    [SerializeField]
    private Camera _camera;
    [SerializeField]
    public Tilemap _floorTilemap;
    [SerializeField]
    public Tilemap _collisionTilemap;
    [SerializeField]
    public Tilemap _uITilemap;

    private static Vector3Int _currentGridPos;
    private static Vector3Int _lastGridPos;

    private static Entity _selectedEntity;
    private static Vector3Int _selectedGridPos;
    private static Vector3 _mousePos;
    private static Vector3Int _tileChosenPos;

    private static bool _isExecutorSelected;
    private void OnEnable()
    {
        Selecting.OnSelectingUpdate += SelectingUpdate;
        ShowRange.OnShowRangeEnter += ShowRangeEnter;
        ChoosingTile.OnChoosingTileUpdate += ChoosingTileUpdate;
        MovingToTile.OnMovingToTileEnter += MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate += MovingToTileUpdate;
    }

    private void OnDisable()
    {
        Selecting.OnSelectingUpdate -= SelectingUpdate;
        ShowRange.OnShowRangeEnter -= ShowRangeEnter;
        ChoosingTile.OnChoosingTileUpdate -= ChoosingTileUpdate;
        MovingToTile.OnMovingToTileEnter -= MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate -= MovingToTileUpdate;
    }

    private void Update()
    {
        _selectedEntity = null;
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
                    
                    EntityManager.SetExecutor(_selectedEntity);
                    _selectedGridPos = _currentGridPos;
                    _isExecutorSelected = true;
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

    private void ShowRangeEnter()
    {
        var range = EntityManager.ExecutorEntity.Range;
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

                    if (pos == _selectedGridPos)
                        _uITilemap.SetTile(pos, _targetTile);
                    else if (CanMove(pos))
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (_floorTilemap.HasTile(pos))
                        _uITilemap.SetTile(pos, _nullTile);
                }
            }
        }
    }
    private bool CanMove(Vector3Int pos)
    {
        if (!_floorTilemap.HasTile(pos) || _collisionTilemap.HasTile(pos))
            return false;
        return true;
    }

    private void ChoosingTileUpdate(Animator animator)
    {
        if (_uITilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos && _rangeTile == _uITilemap.GetTile(_currentGridPos))
        {
            if (!(_lastGridPos == _selectedGridPos))
                _uITilemap.SetTile(_lastGridPos, _rangeTile);
            _uITilemap.SetTile(_currentGridPos, _targetTile);
            _lastGridPos = _currentGridPos;
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
}
