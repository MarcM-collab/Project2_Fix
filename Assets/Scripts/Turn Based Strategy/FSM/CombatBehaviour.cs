using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatBehaviour : MonoBehaviour
{
    internal enum CharType
    {
        Nothing,
        EnemyCharacter,
        AllyCharacter,
        EnemyHero,
        AllyHero
    }

    [SerializeField]
    private Art _art;
    internal Camera _camera => _art.Camera;

    internal Tile _pointingTile => _art.PointingTile;
    internal Tile _rangeTile => _art.RangeTile;
    internal Tile _targetTile => _art.TargetTile;
    internal Tile _nullTile => _art.NullTile;
    internal Tile _allyTile => _art.AllyTile;

    internal Tilemap _floorTilemap => _art.FloorTilemap;
    internal Tilemap _collisionTilemap => _art.CollisionTilemap;
    internal Tilemap _uITilemap => _art.UITilemap;

    internal Texture2D _cursorHand => _art.CursorHand;
    internal Texture2D _cursorSword => _art.CursorSword;
    internal Texture2D _cursorArrow => _art.CursorArrow;

    internal Hero _enemyHero;
    internal List<Vector3Int> _enemyHeroTiles;
    internal List<Vector3Int> _enemyHeroAttackableTiles;

    internal static Character _executorCharacter => EntityManager.ExecutorCharacter;
    internal static Entity _targetEntity => EntityManager.TargetCharacter;
    internal static Vector3Int _executorGridPos;
    internal static Vector3Int _tileChosenPos;
    internal static Vector3Int _targetGridPos;

    private void OnEnable()
    {
        MovingToTile.OnMovingToTileEnter += MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate += MovingToTileUpdate;
        HidePath.OnHidePathEnter += HidePathEnter;
        Attacking.OnAttackingEnter += AttackingEnter;
        Attacking.OnAttackingUpdate += AttackingUpdate;
        Attacking.OnAttackingExit += AttackingExit;
        HideUI.OnHideUIEnter += HideUIEnter;
        ExhaustingAndReset.OnExhaustingAndResetEnter += ExhaustingAndResetEnter;
        ExhaustingAndReset.OnExhaustingAndResetUpdate += ExhaustingAndResetUpdate;
        ExhaustingAndReset.OnExhaustingAndResetExit += ExhaustingAndResetExit;
    }
    private void OnDisable()
    {
        MovingToTile.OnMovingToTileEnter -= MovingToTileEnter;
        MovingToTile.OnMovingToTileUpdate -= MovingToTileUpdate;
        HidePath.OnHidePathEnter -= HidePathEnter;
        Attacking.OnAttackingEnter -= AttackingEnter;
        Attacking.OnAttackingUpdate -= AttackingUpdate;
        Attacking.OnAttackingExit -= AttackingExit;
        HideUI.OnHideUIEnter -= HideUIEnter;
        ExhaustingAndReset.OnExhaustingAndResetEnter -= ExhaustingAndResetEnter;
        ExhaustingAndReset.OnExhaustingAndResetUpdate -= ExhaustingAndResetUpdate;
        ExhaustingAndReset.OnExhaustingAndResetExit -= ExhaustingAndResetExit;
    }
    //----------------------------------------MovingToTileEnter----------------------------------------
    private void MovingToTileEnter()
    {
        //PathFinding
        List<Vector3> list = new List<Vector3>();
        Vector3 v = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2);
        list.Add(_executorGridPos + v);
        list.Add(_tileChosenPos + v);
        _executorCharacter.Path = list;

        var deltaX = list[list.Count - 1].x - list[0].x;

        TurningExecutor(deltaX);

        _executorCharacter.Walking = true;
    }
    private void MovingToTileUpdate(Animator animator)
    {
        animator.SetBool("MovedToTile", !_executorCharacter.Walking);
    }
    //----------------------------------------HidePath----------------------------------------
    private void HidePathEnter()
    {
        HideTilemapElements(_uITilemap, Hide2);
    }
    private bool Hide2(Vector3Int vector)
    {
        return !(_uITilemap.GetTile(vector) == _targetTile);
    }
    //----------------------------------------Attacking----------------------------------------
    private void AttackingEnter()
    {
        var deltaX = _targetGridPos.x - _tileChosenPos.x;
        TurningExecutor(deltaX);
    }
    private void AttackingUpdate(Animator animator)
    {
        animator.SetBool("Attacking", _executorCharacter.Turn);
    }
    private void AttackingExit()
    {
        _executorCharacter.Attack = true;
    }
    //----------------------------------------HideUI----------------------------------------
    private void HideUIEnter()
    {
        HideTilemapElements(_uITilemap, Hide3);
    }
    private bool Hide3(Vector3Int vector)
    {
        return true;
    }
    //----------------------------------------ExhaustingAndReset----------------------------------------
    private void ExhaustingAndResetEnter(Animator animator)
    {
        animator.SetBool("Selected", false);
        animator.SetBool("TileChosen", false);
        animator.SetBool("MovedToTile", false);
        animator.SetBool("Ranged", false);
        animator.SetBool("Attacking", false);
        animator.SetBool("PreparingAttack", false);
        animator.SetBool("NotWaiting", false);

        _executorCharacter.Exhausted = true;

        Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
    private void ExhaustingAndResetUpdate(Animator animator)
    {
        if (_executorCharacter.IsExhaustedAnim && (_targetEntity.IsDeadAnim || _targetEntity.IsAlive))
        {
            animator.SetTrigger("Exhausted");
        }
    }
    private void ExhaustingAndResetExit(Animator animator)
    {
    }
    //----------------------------------------GENERAL FUNCTIONS----------------------------------------
    public void HideTilemapElements(Tilemap tilemap, System.Func<Vector3Int, bool> function)
    {
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);

                if (function(vector))
                    _uITilemap.SetTile(vector, null);
            }
        }
    }
    public void TurningExecutor(float deltaX)
    {
        if (deltaX < 0)
        {
            if (_executorCharacter.transform.rotation.eulerAngles.y == 180)
            {
                _executorCharacter.Turn = true;
            }
        }
        if (deltaX > 0)
        {
            if (_executorCharacter.transform.rotation.eulerAngles.y == 0)
            {
                _executorCharacter.Turn = true;
            }
        }
    }
    public int InTile(Vector3 vector)
    {
        RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Entity is null))
            {
                if (_executorCharacter.Team != gameObject.GetComponent<Entity>().Team)
                    return (int)CharType.EnemyCharacter;
                else
                    return (int)CharType.AllyCharacter;
            }
            else if (!(gameObject.GetComponent("Hero") as Hero is null))
            {
                if (_executorCharacter.Team != gameObject.GetComponent<Hero>().Team)
                    return (int)CharType.EnemyHero;
                else
                    return (int)CharType.AllyHero;
            }
        }
        return (int)CharType.Nothing;
    }

    public void ShowHeroTiles()
    {
        for (int i = 0; i < _enemyHeroTiles.Count; i++)
        {
            _uITilemap.SetTile(_enemyHeroTiles[i], _targetTile);
        }
    }
}
