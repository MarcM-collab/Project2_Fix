using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatAIBehaviour : MonoBehaviour
{
    private Camera _camera => _cCB.Camera;
    private Tilemap _floorTilemap => _cCB.FloorTilemap;
    private Tilemap _collisionTilemap => _cCB.CollisionTilemap;
    private Tilemap _uITilemap => _cCB.UITilemap;

    private Texture2D _cursorHand => _cCB.CursorHand;
    private Texture2D _cursorSword => _cCB.CursorSword;
    private Texture2D _cursorArrow => _cCB.CursorArrow;

    private static Character _executorCharacter => CombatCommonBehaviour.ExecutorCharacter;
    private static Character _targetCharacter => CombatCommonBehaviour.TargetCharacter;

    public CombatCommonBehaviour _cCB;

    private void OnEnable()
    {
        Selecting.OnSelectingEnter += SelectingEnter; //needed
        Melee_ChoosingTile.OnMelee_ChoosingTileEnter += Melee_ChoosingTileEnter; //needed
        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter += Ranged_ChoosingTileEnter; //needed
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter += Melee_ChoosingAttackTileEnter; //needed
    }

    private void OnDisable()
    {
        Selecting.OnSelectingEnter -= SelectingEnter; //needed
        Melee_ChoosingTile.OnMelee_ChoosingTileEnter -= Melee_ChoosingTileEnter; //needed
        Ranged_ChoosingTile.OnRanged_ChoosingTileEnter -= Ranged_ChoosingTileEnter; //needed
        Melee_ChoosingAttackTile.OnMelee_ChoosingAttackTileEnter -= Melee_ChoosingAttackTileEnter; //needed
    }
    private void SelectingEnter()
    {
        throw new NotImplementedException();
    }

    private void Melee_ChoosingTileEnter()
    {
        throw new NotImplementedException();
    }

    private void Ranged_ChoosingTileEnter()
    {
        throw new NotImplementedException();
    }

    private void Melee_ChoosingAttackTileEnter()
    {
        throw new NotImplementedException();
    }
}