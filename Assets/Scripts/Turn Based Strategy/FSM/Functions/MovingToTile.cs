using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTile : CombatBehaviour
{
    private void OnEnable()
    {
        MovingToTileBehaviour.OnMovingToTileEnter += MovingToTileEnter;
        MovingToTileBehaviour.OnMovingToTileUpdate += MovingToTileUpdate;
    }
    private void OnDisable()
    {
        MovingToTileBehaviour.OnMovingToTileEnter -= MovingToTileEnter;
        MovingToTileBehaviour.OnMovingToTileUpdate -= MovingToTileUpdate;
    }
    private void MovingToTileEnter()
    {
        if (_tileChosenGridPosition != _executorGridPosition)
        {
            var cellSize = TileManager.CellSize;
            _executorCharacter.TeleportPoint = _tileChosenGridPosition + cellSize;

            _executorCharacter.Teleporting = true;
        }
    }
    private void MovingToTileUpdate(Animator animator)
    {
        if (_tileChosenGridPosition != _executorGridPosition) 
        { 
            animator.SetBool("MovedToTile", !_executorCharacter.Teleporting);
        }
        else
        {
            animator.SetBool("MovedToTile", true);
        }
    }
}
