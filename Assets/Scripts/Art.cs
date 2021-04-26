using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Art : MonoBehaviour
{
    [Header("Camera")]
    public Camera Camera;

    [Header("UITiles")]
    public Tile PointingTile;
    public Tile RangeTile;
    public Tile TargetTile;
    public Tile NullTile;
    public Tile AllyTile;

    [Header("TileMaps")]
    public Tilemap FloorTilemap;
    public Tilemap CollisionTilemap;
    public Tilemap UITilemap;

    [Header("Cursors")]
    public Texture2D CursorHand;
    public Texture2D CursorSword;
    public Texture2D CursorArrow;
}
