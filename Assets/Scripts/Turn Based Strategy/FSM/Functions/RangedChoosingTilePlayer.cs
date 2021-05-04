using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedChoosingTilePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        RangedChoosingTileBehaviour.OnRangedChoosingTileUpdate += RangedChoosingTileUpdate;
    }
    private void OnDisable()
    {
        RangedChoosingTileBehaviour.OnRangedChoosingTileUpdate -= RangedChoosingTileUpdate;
    }
    private void RangedChoosingTileUpdate(Animator animator)
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

        var TileSelcted = InputManager.LeftMouseClick;
        if (TileSelcted)
        {
            var OutsiedeRangeSelected = !_uITilemap.HasTile(_currentGridPos) || _executorGridPos == _currentGridPos;
            var EnemySelected = _uITilemap.GetTile(_currentGridPos) == _targetTile;
            var InRangeTileSelected = _pointingTile == _uITilemap.GetTile(_currentGridPos);

            if (OutsiedeRangeSelected)
                animator.SetBool("Selected", false);
            else if (EnemySelected)
            {
                _tileChosenGridPosition = _executorGridPos;
                _targetGridPosition = _currentGridPos;
                EntityManager.SetTarget(SelectCharacter());
                EntityManager.SetTarget(_targetEntity);
                animator.SetBool("Attacking", true);
                IsAttacking = true;
                animator.SetTrigger("TileChosen");
            }
            else if (InRangeTileSelected)
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_tileChosenGridPosition, _allyTile);
                animator.SetTrigger("TileChosen");
            }
        }
    }
    private void ChangeCursorIfEnemy()
    {
        if (IsEnemy())
            Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
}
