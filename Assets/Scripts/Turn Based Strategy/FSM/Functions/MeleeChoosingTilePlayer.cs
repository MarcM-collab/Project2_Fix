using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingTilePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileUpdate += MeleeChoosingTileUpdate;
    }
    private void OnDisable()
    {
        MeleeChoosingTileBehaviour.OnMeleeChoosingTileUpdate -= MeleeChoosingTileUpdate;
    }
    private void MeleeChoosingTileUpdate(Animator animator)
    {
        //ChangeCursorIfEnemy();

        var PointingNewTile = _currentGridPos != _lastGridPos;
        var PointingSpawnableTile = _uITilemap.GetTile(_currentGridPos) == _allyTile && _currentGridPos != _executorGridPos;
        var LeavingSpawnableZone = _uITilemap.GetTile(_lastGridPos) == _pointingTile;
        var PointingNewSpawnableTile = _uITilemap.GetTile(_lastGridPos) == _pointingTile;

        if (PointingNewTile)
        {
            if (PointingSpawnableTile)
            {
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
            }
            else
            {
                if (LeavingSpawnableZone)
                {
                    _uITilemap.SetTile(_lastGridPos, _allyTile);
                }
            }
            if (PointingNewSpawnableTile)
            {
                _uITilemap.SetTile(_lastGridPos, _allyTile);
            }
            _lastGridPos = _currentGridPos;
        }

        var TileSelected = InputManager.LeftMouseClick;
        if (TileSelected)
        {
            var OutsiedeRangeSelected = !_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos;
            var EnemySelected = _uITilemap.GetTile(_currentGridPos) == _targetTile;
            var InRangeTileSelected = _pointingTile == _uITilemap.GetTile(_currentGridPos);

            if (OutsiedeRangeSelected)
                animator.SetBool("Selected", false);

            else if (EnemySelected)
            {
                _targetGridPosition = _currentGridPos;
                EntityManager.SetTarget(SelectEntity());
                Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
                animator.SetBool("PreparingAttack", true);
            }
            else if (InRangeTileSelected)
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_currentGridPos, _allyTile);
                animator.SetTrigger("TileChosen");
            }
        }
    }
    private void ChangeCursorIfEnemy()
    {
        if (IsEnemy())
        {
            Cursor.SetCursor(_cursorSword, Vector2.zero, CursorMode.Auto);
        }
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
}
