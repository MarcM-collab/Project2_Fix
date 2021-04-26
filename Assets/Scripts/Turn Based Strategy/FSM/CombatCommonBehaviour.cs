using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CharType
{
    Nothing,
    EnemyCharacter,
    AllyCharacter,
    EnemyHero,
    AllyHero
}

public class CombatCommonBehaviour : MonoBehaviour
{
    public Tile PointingTile;
    public Tile RangeTile;
    public Tile TargetTile;
    public Tile NullTile;
    public Tile AllyTile;

    public Camera Camera;
    public Tilemap FloorTilemap;
    public Tilemap CollisionTilemap;
    public Tilemap UITilemap;

    public Texture2D CursorHand;
    public Texture2D CursorSword;
    public Texture2D CursorArrow;

    public static Character ExecutorCharacter => EntityManager.ExecutorCharacter;
    public static Entity TargetCharacter => EntityManager.TargetCharacter;
    public static Vector3Int ExecutorGridPos { get; set;}
    public static Vector3Int TileChosenPos { get; set; }
    public static Vector3Int TargetGridPos { get; set; }

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
        Vector3 v = new Vector3(FloorTilemap.cellSize.x / 2, FloorTilemap.cellSize.y / 2);
        list.Add(ExecutorGridPos + v);
        list.Add(TileChosenPos + v);
        ExecutorCharacter.Path = list;

        var deltaX = list[list.Count - 1].x - list[0].x;

        TurningExecutor(deltaX);

        ExecutorCharacter.Walking = true;
    }
    private void MovingToTileUpdate(Animator animator)
    {
        animator.SetBool("MovedToTile", !ExecutorCharacter.Walking);
    }
    //----------------------------------------HidePath----------------------------------------
    private void HidePathEnter()
    {
        HideTilemapElements(UITilemap, Hide2);
    }
    private bool Hide2(Vector3Int vector)
    {
        return !(UITilemap.GetTile(vector) == TargetTile);
    }
    //----------------------------------------Attacking----------------------------------------
    private void AttackingEnter()
    {
        var deltaX = TargetGridPos.x - TileChosenPos.x;
        TurningExecutor(deltaX);
    }
    private void AttackingUpdate(Animator animator)
    {
        animator.SetBool("Attacking", ExecutorCharacter.Turn);
    }
    private void AttackingExit()
    {
        ExecutorCharacter.Attack = true;
    }
    //----------------------------------------HideUI----------------------------------------
    private void HideUIEnter()
    {
        HideTilemapElements(UITilemap, Hide3);
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

        ExecutorCharacter.Exhausted = true;

        Cursor.SetCursor(CursorHand, Vector2.zero, CursorMode.Auto);
    }
    private void ExhaustingAndResetUpdate(Animator animator)
    {
        if (ExecutorCharacter.IsExhaustedAnim && (TargetCharacter.IsDeadAnim || TargetCharacter.IsAlive))
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
                    UITilemap.SetTile(vector, null);
            }
        }
    }
    public void TurningExecutor(float deltaX)
    {
        if (deltaX < 0)
        {
            if (ExecutorCharacter.transform.rotation.eulerAngles.y == 180)
            {
                ExecutorCharacter.Turn = true;
            }
        }
        if (deltaX > 0)
        {
            if (ExecutorCharacter.transform.rotation.eulerAngles.y == 0)
            {
                ExecutorCharacter.Turn = true;
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
                if (ExecutorCharacter.Team != gameObject.GetComponent<Entity>().Team)
                    return (int)CharType.EnemyCharacter;
                else
                    return (int)CharType.AllyCharacter;
            }
            else if (!(gameObject.GetComponent("Hero") as Hero is null))
            {
                if (ExecutorCharacter.Team != gameObject.GetComponent<Hero>().Team)
                    return (int)CharType.EnemyHero;
                else
                    return (int)CharType.AllyHero;
            }
        }
        return (int)CharType.Nothing;
    }

    public void ShowHeroTiles(List<Vector3Int> heroTiles)
    {
        for (int i = 0; i < heroTiles.Count; i++)
        {
            UITilemap.SetTile(heroTiles[i], TargetTile);
        }
    }
}
