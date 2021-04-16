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
    private Camera _camera;
    [SerializeField]
    public Tilemap _floorTilemap;
    [SerializeField]
    public Tilemap _collisionTilemap;
    [SerializeField]
    public Tilemap _uITilemap;

    private static Vector3Int _currentGridPos;
    private static Vector3Int _lastGridPos;

    public static Entity SelectedEntity;
    private static Vector3Int _selectedGridPos;
    private static Vector3 _mousePos;
    private void OnEnable()
    {
        Selecting.OnSelectingUpdate += SelectingUpdate;
    }

    private void OnDisable()
    {
        Selecting.OnSelectingUpdate -= SelectingUpdate;
    }

    private void Update()
    {
        SelectedEntity = null;
        _mousePos = Input.mousePosition;
        _currentGridPos = _floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(_mousePos));
    }

    private void SelectingUpdate(Animator animator)
    {
        if (_floorTilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos)
        {
            _uITilemap.SetTile(_lastGridPos, null);
            _uITilemap.SetTile(_currentGridPos, _pointingTile);
            _lastGridPos = _currentGridPos;
        }
        if (InputManager.LeftMouseClick)
        {
            var entity = SelectEntity();
            if (!(entity is null))
            {
                if (EntityManager.IsEntityInList(EntityManager.GetActiveAllies(entity), entity))
                {
                    EntityManager.SetExecutor(entity);
                    SelectedEntity = entity;
                    _selectedGridPos = _currentGridPos;
                    animator.SetTrigger("Selected");
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
        for (int j = -range; j <= range; j++)
        {
            var numberOfTiles = 1 + 2 * (Mathf.Abs(j) - range);
            for (int i = -range; i <= range; i++)
            {
                Debug.Log(numberOfTiles - range < numberOfTiles);
                if (numberOfTiles - range < numberOfTiles)
                {
                    Vector3Int vector = new Vector3Int(i, j, 0);
                    var pos = _selectedGridPos + vector;
                    if (CanMove(pos))
                        _uITilemap.SetTile(pos, _rangeTile);
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
}
