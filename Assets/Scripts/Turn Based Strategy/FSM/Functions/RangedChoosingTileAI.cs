using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedChoosingTileAI : CombatAIBehaviour
{
    private void OnEnable()
    {
        RangedChoosingTileBehaviour.OnRangedChoosingTileEnter += RangedChoosingTileEnter;
    }
    private void OnDisable()
    {
        RangedChoosingTileBehaviour.OnRangedChoosingTileEnter -= RangedChoosingTileEnter;
    }
    private void RangedChoosingTileEnter(Animator animator)
    {
        var characters = EntityManager.GetLivingCharacters(Team.TeamPlayer);

        EntityManager.SetTarget(characters[0]);
        _targetGridPosition = _floorTilemap.WorldToCell(characters[0].transform.position);
        _uITilemap.SetTile(_targetGridPosition, _targetTile);

        _tileChosenGridPosition = _executorGridPos;

        animator.SetBool("Attacking", true);
        animator.SetTrigger("TileChosen");
    }
}
