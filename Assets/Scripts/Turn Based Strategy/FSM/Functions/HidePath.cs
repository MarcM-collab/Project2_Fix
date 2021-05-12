using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePath : CombatBehaviour
{
    private void OnEnable()
    {
        HidePathBehaviour.OnHidePathEnter += HidePathEnter;
    }
    private void OnDisable()
    {
        HidePathBehaviour.OnHidePathEnter += HidePathEnter;
    }
    private void HidePathEnter()
    {
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap, null, IsPath);
    }
    private bool IsPath(Vector3Int vector)
    {
        return !(_uITilemap.GetTile(vector) == _targetTile);
    }
}
