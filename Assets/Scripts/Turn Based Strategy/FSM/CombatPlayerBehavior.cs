using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatPlayerBehavior : MonoBehaviour
{
    private Tile _pointingTile => _cCB.PointingTile;
    private Tile _rangeTile => _cCB.RangeTile;
    private Tile _targetTile => _cCB.TargetTile;
    private Tile _nullTile => _cCB.NullTile;
    private Tile _allyTile => _cCB.AllyTile;


    private Camera _camera => _cCB.Camera;
    private Tilemap _floorTilemap => _cCB.FloorTilemap;
    private Tilemap _collisionTilemap => _cCB.CollisionTilemap;
    private Tilemap _uITilemap => _cCB.UITilemap;

    private Texture2D _cursorHand => _cCB.CursorHand;
    private Texture2D _cursorSword => _cCB.CursorSword;
    private Texture2D _cursorArrow => _cCB.CursorArrow;

    private static Character _executorCharacter => CombatCommonBehaviour.ExecutorCharacter;
    private static Character _targetCharacter => CombatCommonBehaviour.TargetCharacter;

    public CombatCommonBehaviour _cCB;

    private static Vector3Int _currentGridPos;
    private static Vector3Int _lastGridPos;
    private static Vector3 _mousePos;

    private static bool _isExecutorSelected;
    private void OnEnable()
    {
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
        Selecting.OnSelectingUpdate -= SelectingUpdate;
        Selecting.OnSelectingExit -= SelectingExit;
        Melee_ShowRange.OnMelee_ShowRangeEnter -= Melee_ShowRangeEnter;
        Ranged_ShowRange.OnRanged_ShowRangeEnter -= Ranged_ShowRangeEnter;
        Melee_ChoosingTile.OnMelee_ChoosingTileUpdate -= Melee_ChoosingTileUpdate;
        Ranged_ChoosingTile.OnRanged_ChoosingTileUpdate -= Ranged_ChoosingTileUpdate;
        HideRange.OnHideRangeEnter -= HideRangeEnter;
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
            var tempCharacter = SelectCharacter();
            if (!(tempCharacter is null))
            {
                if (CharacterManager.IsEntityInList(CharacterManager.GetActiveAllies(tempCharacter), tempCharacter))
                {
                    CharacterManager.SetExecutor(tempCharacter);
                    CombatCommonBehaviour.ExecutorGridPos = _currentGridPos;
                    _isExecutorSelected = true;
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
        _isExecutorSelected = false;
    }
    //----------------------------------------Melee_ShowRange----------------------------------------
    private void Melee_ShowRangeEnter()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        var range = _executorCharacter.Range + 1;
        int counter = 0;

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
                    if (InTile(vToW) == 1)
                        _uITilemap.SetTile(pos, _targetTile);
                    else if (CanMove(pos) && InTile(vToW) == 0)
                        _uITilemap.SetTile(pos, _rangeTile);
                    else if (pos == CombatCommonBehaviour.ExecutorGridPos || InTile(vToW) == 2)
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
                    var pos = CombatCommonBehaviour.ExecutorGridPos + vector;
                    Vector3 vToW = pos + v;

                    if (pos == CombatCommonBehaviour.ExecutorGridPos || InTile(vToW) == 2)
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
    }    //----------------------------------------HidePathEnter----------------------------------------
    private static int InTile(Vector3 vector)
    {
        RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                if (_executorCharacter.Team != gameObject.GetComponent<Character>().Team)
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
            if (!_uITilemap.HasTile(_currentGridPos) || CombatCommonBehaviour.ExecutorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);
            else if (_targetTile == _uITilemap.GetTile(_currentGridPos))
            {
                CombatCommonBehaviour.TileChosenPos = CombatCommonBehaviour.ExecutorGridPos;
                CombatCommonBehaviour.TargetGridPos = _currentGridPos;
                CharacterManager.SetTarget(SelectCharacter());
                CharacterManager.SetTarget(_targetCharacter);
                animator.SetBool("Attacking", true);
                animator.SetTrigger("TileChosen");
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                CombatCommonBehaviour.TileChosenPos = _currentGridPos;
                _uITilemap.SetTile(CombatCommonBehaviour.TileChosenPos, _allyTile);
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
            if (!_uITilemap.HasTile(_currentGridPos) || CombatCommonBehaviour.ExecutorGridPos == _currentGridPos)
                animator.SetBool("Selected", false);
            else if (_targetTile == _uITilemap.GetTile(_currentGridPos))
            {
                CombatCommonBehaviour.TargetGridPos = _currentGridPos;
                CharacterManager.SetTarget(SelectCharacter());
                CharacterManager.SetTarget(_targetCharacter);
                Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
                animator.SetBool("PreparingAttack", true);
            }
            else if (_pointingTile == _uITilemap.GetTile(_currentGridPos))
            {
                CombatCommonBehaviour.TileChosenPos = _currentGridPos;
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
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                var pos = CombatCommonBehaviour.TargetGridPos + vector;
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
                //CharacterManager.SetTarget(null);
                animator.SetBool("PreparingAttack", false);
            }

            else
            {
                CombatCommonBehaviour.TileChosenPos = _currentGridPos;
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
            if ((!_uITilemap.HasTile(_currentGridPos) || _allyTile == _uITilemap.GetTile(_currentGridPos) || _nullTile == _uITilemap.GetTile(_currentGridPos) || _rangeTile == _uITilemap.GetTile(_currentGridPos) || CombatCommonBehaviour.TargetGridPos == _currentGridPos)
            && _uITilemap.HasTile(_lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_lastGridPos) && _allyTile != _uITilemap.GetTile(_lastGridPos) && _nullTile != _uITilemap.GetTile(_lastGridPos)) //only _pointing
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
                _lastGridPos = _currentGridPos;
            }
            //outside to range grid
            else if (_uITilemap.HasTile(_currentGridPos)
            && (!_uITilemap.HasTile(_lastGridPos) || _rangeTile == _uITilemap.GetTile(_lastGridPos) || _allyTile == _uITilemap.GetTile(_lastGridPos) || _nullTile == _uITilemap.GetTile(_lastGridPos) || CombatCommonBehaviour.TargetGridPos == _lastGridPos)
            && _rangeTile != _uITilemap.GetTile(_currentGridPos) && _allyTile != _uITilemap.GetTile(_currentGridPos) && _nullTile != _uITilemap.GetTile(_currentGridPos) && !IsEnemy()) //only _range
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (_uITilemap.HasTile(_currentGridPos) && _targetTile == _uITilemap.GetTile(_currentGridPos) && CombatCommonBehaviour.TargetGridPos != _lastGridPos && CombatCommonBehaviour.TargetGridPos != _currentGridPos
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
                var pos = CombatCommonBehaviour.TargetGridPos + vector;
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
        _cCB.HideTilemapElements(_uITilemap, Hide1);
    }
    private bool Hide1(Vector3 vector)
    {
        return !(CombatCommonBehaviour.ExecutorGridPos == vector || CombatCommonBehaviour.TileChosenPos == vector || CombatCommonBehaviour.TargetGridPos == vector);
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
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                if (CharacterManager.ExecutorCharacter.Team != gameObject.GetComponent<Character>().Team)
                    return true;
            }
        }
        return false;
    }
}
