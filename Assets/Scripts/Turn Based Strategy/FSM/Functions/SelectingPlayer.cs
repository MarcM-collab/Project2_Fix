using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingPlayer : CombatPlayerBehaviour
{
    private void OnEnable()
    {
        SelectingBehaviour.OnSelectingUpdate += SelectingUpdate;
        SelectingBehaviour.OnSelectingExit += SelectingExit;
    }
    private void OnDisable()
    {
        SelectingBehaviour.OnSelectingUpdate -= SelectingUpdate;
        SelectingBehaviour.OnSelectingExit -= SelectingExit;
    }
    private void SelectingUpdate(Animator animator)
    {
        var PointingNewFloorTile = _floorTilemap.HasTile(_currentGridPos) && _currentGridPos != _lastGridPos;
        var PointingOutOfFloor = !_floorTilemap.HasTile(_currentGridPos);

        if (!animator.GetBool("Selected"))
        {
            if (PointingNewFloorTile)
            {
                _uITilemap.SetTile(_lastGridPos, null);
                _uITilemap.SetTile(_currentGridPos, _pointingTile);
                _lastGridPos = _currentGridPos;
            }
            else if (PointingOutOfFloor)
            {
                _uITilemap.SetTile(_lastGridPos, null);
            }
        }

        var TileSelected = InputManager.LeftMouseClick;

        if (TileSelected)
        {
            var tempCharacter = SelectCharacter();
            var PointingACharacter = !(tempCharacter is null);

            if (PointingACharacter)
            {
                var CharacterOfTeamPlayer = EntityManager.IsEntityInList(EntityManager.GetActiveCharacters(Team.TeamPlayer), tempCharacter);

                if (CharacterOfTeamPlayer)
                {
                    EntityManager.SetExecutor(tempCharacter);
                    _executorGridPos = _currentGridPos;

                    var CharacterIsRanged = tempCharacter.Class == Class.Ranged;

                    if (CharacterIsRanged)
                    {
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
}
