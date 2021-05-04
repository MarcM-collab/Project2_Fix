using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatBehaviour : MonoBehaviour
{
    internal enum EntityType
    {
        Nothing,
        EnemyCharacter,
        AllyCharacter,
        EnemyHero,
        AllyHero
    }

    protected Art _art => FindObjectOfType<Art>();
    protected TileManager _tileManager => FindObjectOfType<TileManager>();
    protected Camera _camera => _art.Camera;

    protected Tile _pointingTile => _tileManager.PointingTile;
    protected Tile _targetTile => _tileManager.TargetTile;
    protected Tile _allyTile => _tileManager.AllyTile;

    protected Tilemap _floorTilemap => _tileManager.FloorTilemap;
    protected Tilemap _collisionTilemap => _tileManager.CollisionTilemap;
    protected Tilemap _uITilemap => _tileManager.UITilemap;

    protected Texture2D _cursorHand => _art.CursorHand;
    protected Texture2D _cursorSword => _art.CursorSword;
    protected Texture2D _cursorArrow => _art.CursorArrow;

    protected Hero _enemyHero;
    protected List<Vector3Int> _enemyHeroTiles;
    protected List<Vector3Int> _enemyHeroAttackableTiles;

    protected static Character _executorCharacter => EntityManager.ExecutorCharacter;
    protected static Entity _targetEntity => EntityManager.TargetCharacter;
    protected static Vector3Int _executorGridPos;
    protected static Vector3Int _tileChosenGridPosition;
    protected static Vector3Int _targetGridPosition;

    protected int InTile(Vector3 vector)
    {
        RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Entity is null))
            {
                if (TurnManager.TeamTurn != gameObject.GetComponent<Entity>().Team)
                    return (int)EntityType.EnemyCharacter;
                else
                    return (int)EntityType.AllyCharacter;
            }
            else if (!(gameObject.GetComponent("Hero") as Hero is null))
            {
                if (TurnManager.TeamTurn != gameObject.GetComponent<Hero>().Team)
                    return (int)EntityType.EnemyHero;
                else
                    return (int)EntityType.AllyHero;
            }
        }
        return (int)EntityType.Nothing;
    }

    protected bool anyUnitInTile(Vector3 pos)
    {
        return InTile(pos) == (int)EntityType.EnemyCharacter || InTile(pos) == (int)EntityType.AllyCharacter;
    }

    protected void ShowHeroTiles()
    {
        for (int i = 0; i < _enemyHeroTiles.Count; i++)
        {
            _uITilemap.SetTile(_enemyHeroTiles[i], _targetTile);
        }
    }
    protected void HideHeroTiles()
    {
        for (int i = 0; i < _enemyHeroTiles.Count; i++)
        {
            _uITilemap.SetTile(_enemyHeroTiles[i], null);
        }
    }
}
