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
        List<Vector3> path = new List<Vector3>();

        PathFinding(path);

        var deltaX = path[path.Count - 1].x - path[0].x;
        _executorCharacter.TurningExecutor(deltaX);

        _executorCharacter.Walking = true;
    }
    private void MovingToTileUpdate(Animator animator)
    {
        animator.SetBool("MovedToTile", !_executorCharacter.Walking);
    }
    private void PathFinding(List<Vector3> path)
    {
        var cellSize = TileManager.CellSize;
        path.Add(_executorGridPos + cellSize);
        path.Add(_tileChosenGridPosition + cellSize);
        _executorCharacter.Path = path;
    }
}
