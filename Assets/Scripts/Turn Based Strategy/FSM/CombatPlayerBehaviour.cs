using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatPlayerBehaviour : CombatBehaviour
{
    protected CardUsage _cardUsage => FindObjectOfType<CardUsage>();

    protected static Vector3Int _currentGridPos;
    protected static Vector3Int _lastGridPos;
    protected static Vector3 _mousePos;

    protected static List<Vector3Int> _spawnableTilesEdges = new List<Vector3Int>();
    private void Start()
    {
        _enemyHero = EntityManager.GetHero(Team.TeamAI);
        _enemyHeroAttackableTiles = new List<Vector3Int> { new Vector3Int(3, 1, 0), new Vector3Int(3, 0, 0), new Vector3Int(3, -1, 0), new Vector3Int(3, -2, 0) };
        _spawnableTilesEdges.Add(new Vector3Int(-4, -2, 0));
        _spawnableTilesEdges.Add(new Vector3Int(-3, 1, 0));
    }
    private void Update()
    {
        _mousePos = Input.mousePosition;
        _currentGridPos = _floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(_mousePos));
        _currentGridPos = new Vector3Int(_currentGridPos.x, _currentGridPos.y, 0);
    }
    protected Character SelectCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                return gameObject.GetComponent<Character>();
            }
        }
        return null;
    }
    public static Entity SelectEntity()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                return gameObject.GetComponent<Entity>();
            }
        }
        return null;
    }

    public static bool IsEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_mousePos), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Entity is null))
            {
                if (EntityManager.ExecutorCharacter.Team != gameObject.GetComponent<Entity>().Team)
                    return true;
            }
        }
        return false;
    }
    protected bool CanMove(Vector3Int pos)
    {
        if (!_floorTilemap.HasTile(pos) || _collisionTilemap.HasTile(pos))
            return false;
        return true;
    }
}
