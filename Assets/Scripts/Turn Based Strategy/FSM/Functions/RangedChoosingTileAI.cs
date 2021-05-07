using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedChoosingTileAI : CombatAIBehaviour
{
    Character[] characters;
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
        characters = EntityManager.GetLivingCharacters(Team.TeamPlayer);
        if (characters.Length > 0)
        {
            AttackEnemy();
            animator.SetBool("Attacking", true);
            animator.SetTrigger("TileChosen");
        }
        else
        {
            _executorCharacter.Exhausted = true;
            foreach (AnimatorControllerParameter paramater in animator.parameters)
            {
                if (paramater.type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool(paramater.name, false);
                }
            }
        }
    }
    private void AttackEnemy()
    {
        EntityManager.SetTarget(characters[0]);
        _targetGridPosition = _floorTilemap.WorldToCell(characters[0].transform.position);
        _uITilemap.SetTile(_targetGridPosition, _targetTile);

        _tileChosenGridPosition = _executorGridPosition;
    }
}
