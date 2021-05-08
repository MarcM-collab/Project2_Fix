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
        var PointingSpawnableTile = _uITilemap.GetTile(_currentGridPos) == _allyTile && _currentGridPos != _executorGridPosition;
        var LeavingSpawnableZone = _uITilemap.GetTile(_lastGridPos) == _movingTile;
        var PointingNewSpawnableTile = _uITilemap.GetTile(_lastGridPos) == _movingTile;

        if (PointingNewTile)
        {
            if (PointingSpawnableTile)
            {
                _uITilemap.SetTile(_currentGridPos, _movingTile);
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
            var CharacterSelected = _uITilemap.GetTile(_currentGridPos) == _targetTile;
            var HeroSelected = _aIHeroTile.activeSelf && InTile(_currentGridPos + TileManager.CellSize) == (int)EntityType.EnemyHero;
            var InRangeTileSelected = _movingTile == _uITilemap.GetTile(_currentGridPos);

            if (CharacterSelected || HeroSelected)
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
            else
            {
                animator.SetBool("Selected", false);
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
