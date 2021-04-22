//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class CombatAIBehaviour : MonoBehaviour
//{
//    private Tile _pointingTile => CombatCommonBehaviour.PointingTile;
//    private Tile _rangeTile => CombatCommonBehaviour.RangeTile;
//    private Tile _targetTile => CombatCommonBehaviour.TargetTile;
//    private Tile _nullTile => CombatCommonBehaviour.NullTile;
//    private Tile _allyTile => CombatCommonBehaviour.AllyTile;


//    private Camera _camera => CombatCommonBehaviour.Camera;
//    private Tilemap _floorTilemap => CombatCommonBehaviour.FloorTilemap;
//    private Tilemap _collisionTilemap => CombatCommonBehaviour.CollisionTilemap;
//    private Tilemap _uITilemap => CombatCommonBehaviour.UITilemap;

//    private static Character _executorCharacter => CombatCommonBehaviour.ExecutorCharacter;
//    private static Character _targetCharacter => CombatCommonBehaviour.TargetCharacter;

//    private void OnEnable()
//    {
//        Selecting.OnSelectingEnter += SelectingEnter; //needed
//        Melee_ChoosingTile.OnMelee_ChoosingTileEnter += Melee_ChoosingTileEnter; //needed
//        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter += Ranged_ChoosingTileEnter; //needed
//        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter += Melee_ChoosingAttackTileEnter; //needed
//        MovingToTile.OnMovingToTileEnter += MovingToTileEnter;
//        MovingToTile.OnMovingToTileUpdate += MovingToTileUpdate;
//        HidePath.OnHidePathEnter += HidePathEnter;
//        Attacking.OnAttackingEnter += AttackingEnter;
//        Attacking.OnAttackingUpdate += AttackingUpdate;
//        Attacking.OnAttackingExit += AttackingExit;
//        HideUI.OnHideUIEnter += HideUIEnter;
//        ExhaustingAndReset.OnExhaustingAndResetEnter += ExhaustingAndResetEnter;
//        ExhaustingAndReset.OnExhaustingAndResetUpdate += ExhaustingAndResetUpdate;
//    }
//    private void OnDisable()
//    {
//        Selecting.OnSelectingEnter -= SelectingEnter; //needed
//        Melee_ChoosingTile.OnMelee_ChoosingTileEnter -= Melee_ChoosingTileEnter; //needed
//        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter -= Ranged_ChoosingTileEnter; //needed
//        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter -= Melee_ChoosingAttackTileEnter; //needed
//        MovingToTile.OnMovingToTileEnter -= MovingToTileEnter;
//        MovingToTile.OnMovingToTileUpdate -= MovingToTileUpdate;
//        HidePath.OnHidePathEnter -= HidePathEnter;
//        Attacking.OnAttackingEnter -= AttackingEnter;
//        Attacking.OnAttackingUpdate -= AttackingUpdate;
//        Attacking.OnAttackingExit -= AttackingExit;
//        HideUI.OnHideUIEnter -= HideUIEnter;
//        ExhaustingAndReset.OnExhaustingAndResetEnter -= ExhaustingAndResetEnter;
//        ExhaustingAndReset.OnExhaustingAndResetUpdate -= ExhaustingAndResetUpdate;
//    }
//}