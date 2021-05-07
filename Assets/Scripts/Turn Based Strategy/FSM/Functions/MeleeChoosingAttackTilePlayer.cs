using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingAttackTilePlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileUpdate += MeleeChoosingAttackTileUpadate;
    }
    private void OnDisable()
    {
        MeleeChoosingAttackTileBehaviour.OnMeleeChoosingAttackTileUpdate += MeleeChoosingAttackTileUpadate;
    }
    private void MeleeChoosingAttackTileUpadate(Animator animator)
    {
        var PointingNewTile = _currentGridPos != _lastGridPos;
        var PointingSpawnableTile = _uITilemap.GetTile(_currentGridPos) == _targetTile && _currentGridPos != _targetGridPosition;
        var LeavingSpawnableZone = _uITilemap.GetTile(_lastGridPos) == _attackingTile;
        var PointingNewSpawnableTile = _uITilemap.GetTile(_lastGridPos) == _attackingTile;

        if (PointingNewTile)
        {
            if (PointingSpawnableTile)
            {
                _uITilemap.SetTile(_currentGridPos, _attackingTile);
            }
            else
            {
                if (LeavingSpawnableZone)
                {
                    _uITilemap.SetTile(_lastGridPos, _targetTile);
                }
            }
            if (PointingNewSpawnableTile)
            {
                _uITilemap.SetTile(_lastGridPos, _targetTile);
            }
            _lastGridPos = _currentGridPos;
        }

        var TileSelected = InputManager.LeftMouseClick;
        if (TileSelected)
        {
            var AttackTileNotSelcted = _attackingTile != _uITilemap.GetTile(_currentGridPos) || IsEnemy();
            if (AttackTileNotSelcted)
            {
                animator.SetBool("PreparingAttack", false);
            }

            else
            {
                _tileChosenGridPosition = _currentGridPos;
                _uITilemap.SetTile(_currentGridPos, _allyTile);
                animator.SetTrigger("TileChosen");
                animator.SetBool("Attacking", true);
            }
        }
    }
}
