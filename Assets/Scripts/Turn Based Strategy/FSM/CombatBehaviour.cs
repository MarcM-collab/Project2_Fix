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
    protected Tile _spawningTile => _tileManager.SpawningTile;
    protected Tile _spawningSelectedTile => _tileManager.SpawningSelectedTile;
    protected Tile _movingTile => _tileManager.MovingTile;
    protected Tile _movingSelectedTile => _tileManager.MovingSelectedTile;
    protected Tile _attackingTile => _tileManager.AttackingTile;
    protected Tile _attackingSelectedTile => _tileManager.AttackingSelectedTile;
    protected Tile _collisionAllyTile => _tileManager.CollisionAllyTile;

    protected GameObject _aIHeroTile => _tileManager.AIHeroTile;
    protected GameObject _playerHeroTile => _tileManager.PlayerHeroTile;

    protected Tilemap _floorTilemap => _tileManager.FloorTilemap;
    protected Tilemap _collisionTilemap => _tileManager.CollisionTilemap;
    protected Tilemap _uITilemap => _tileManager.UITilemap;

    protected Texture2D _cursorHand => _art.CursorHand;
    protected Texture2D _cursorSword => _art.CursorSword;
    protected Texture2D _cursorArrow => _art.CursorArrow;

    protected Hero _enemyHero;
    protected List<Vector3Int> _enemyHeroAttackableTiles;

    protected static Character _executorCharacter => EntityManager.ExecutorCharacter;
    protected static Entity _targetEntity => EntityManager.TargetCharacter;
    protected static Vector3Int _executorGridPosition;
    protected static Vector3Int _tileChosenGridPosition;
    protected static Vector3Int _targetGridPosition;

    protected int InTile(Vector3 vector)
    {
        var postion = new Vector2(vector.x, vector.y);
        RaycastHit2D hit = Physics2D.Raycast(postion, Vector2.zero,Mathf.Infinity);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                if (TurnManager.TeamTurn != gameObject.GetComponent<Character>().Team)
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
        if (TurnManager.TeamTurn == Team.TeamAI)
        {
            _playerHeroTile.SetActive(true);
        }
        else
        {
            _aIHeroTile.SetActive(true);
        }
    }
    protected void HideHeroTiles()
    {
        if (TurnManager.TeamTurn == Team.TeamAI)
        {
            _playerHeroTile.SetActive(false);
        }
        else
        {
            _aIHeroTile.SetActive(false);
        }
    }
}
