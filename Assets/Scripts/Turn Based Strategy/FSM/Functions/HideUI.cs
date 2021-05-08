using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : CombatBehaviour
{
    private void OnEnable()
    {
        HideUIBehaviour.OnHideUIEnter += HideUIEnter;
    }
    private void OnDisable()
    {
        HideUIBehaviour.OnHideUIEnter -= HideUIEnter;
    }
    private void HideUIEnter()
    {
        TileManager.ShowTilesInTilemap(_uITilemap, _uITilemap,null, Everything);
        HideHeroTiles();
    }
    private bool Everything(Vector3Int vector)
    {
        return true;
    }
}
