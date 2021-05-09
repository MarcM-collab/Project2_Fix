using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningPlayer : CombatPlayerBehaviour
{
    Animator anim;
    private void OnEnable()
    {
        SpawningBehaviour.OnSpawningEnter += SpawningEnter;
        SpawningBehaviour.OnSpawningUpdate += SpawningUpdate;
        SpawningBehaviour.OnSpawningExit += SpawningExit;
        ScriptButton.endDrag += ConfirmSpawn;
    }
    private void OnDisable()
    {
        SpawningBehaviour.OnSpawningEnter -= SpawningEnter;
        SpawningBehaviour.OnSpawningUpdate -= SpawningUpdate;
        SpawningBehaviour.OnSpawningExit -= SpawningExit;
        ScriptButton.endDrag -= ConfirmSpawn;
    }
    private void SpawningEnter()
    {
        ShowSpawnableTiles();
    }
    private void SpawningUpdate(Animator animator)
    {
        if (!anim)
            anim = animator;

        if (animator.GetBool("IsDragging"))
        {
            var PointingNewTile = _currentGridPos != _lastGridPos;
            var PointingSpawnableTile = _uITilemap.GetTile(_currentGridPos) == _allyTile;
            var LeavingSpawnableZone = _uITilemap.GetTile(_lastGridPos) == _spawningTile;
            var PointingNewSpawnableTile = _uITilemap.GetTile(_lastGridPos) == _spawningTile;

            if (PointingNewTile)
            {
                if (PointingSpawnableTile)
                {
                    _uITilemap.SetTile(_currentGridPos, _spawningTile);
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
        }
    }
    private void ConfirmSpawn()
    {
        var IsInSpawnableTile = _uITilemap.GetTile(_currentGridPos) == _spawningTile;

        if (IsInSpawnableTile)
        {
            _cardUsage.Spawn();
        }
        if (anim)
            anim.SetBool("IsDragging", false);

        CardUsage.isDragging = false;
    }
    private void SpawningExit()
    {
        HideSpawnableTiles();
    }
    private void ShowSpawnableTiles()
    {
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        for (int i = _spawnableTilesEdges[0].x; i <= _spawnableTilesEdges[1].x; i++)
        {
            for (int j = _spawnableTilesEdges[0].y; j <= _spawnableTilesEdges[1].y; j++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                Vector3 vToW = vector + v;
                if (InTile(vToW) == (int)EntityType.Nothing)
                    _uITilemap.SetTile(vector, _allyTile);
            }
        }
    }
    private void HideSpawnableTiles()
    {
        for (int i = _spawnableTilesEdges[0].x; i <= _spawnableTilesEdges[1].x; i++)
        {
            for (int j = _spawnableTilesEdges[0].y; j <= _spawnableTilesEdges[1].y; j++)
            {
                Vector3Int vector = new Vector3Int(i, j, 0);
                _uITilemap.SetTile(vector, null);
            }
        }
    }
}
