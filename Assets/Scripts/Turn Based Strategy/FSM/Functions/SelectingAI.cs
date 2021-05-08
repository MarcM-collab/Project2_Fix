using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingAI : CombatAIBehaviour
{
    private bool _spawned;
    private void OnEnable()
    {
        SelectingBehaviour.OnSelectingEnter += SelectingEnter;
        SelectingBehaviour.OnSelectingUpdate += SelectingUpdate;
    }
    private void OnDisable()
    {
        SelectingBehaviour.OnSelectingEnter -= SelectingEnter;
        SelectingBehaviour.OnSelectingUpdate -= SelectingUpdate;
    }
    private void SelectingEnter(Animator animator)
    {
    }
    private void SelectingUpdate(Animator animator)
    {
        var CharactersActive = EntityManager.GetActiveCharacters(Team.TeamAI).Length > 0;
        if (!TurnManager.CardDrawn)
        {
            animator.SetBool("ChooseCard", true);
        }
        else if (!TurnManager.Spawned)
        {
            animator.SetBool("IsDragging", true);
        }
        else
        {
            if (CharactersActive)
            {
                var characters = EntityManager.GetActiveCharacters(Team.TeamAI);

                var _currentGridPos = _floorTilemap.WorldToCell(characters[0].transform.position);

                if (characters[0].Class == Class.Ranged)
                {
                    animator.SetBool("Ranged", true);
                }
                else
                {
                    animator.SetBool("Ranged", false);
                }
                animator.SetBool("Selected", true);

                _executorGridPosition = _currentGridPos;
                _uITilemap.SetTile(_executorGridPosition, _allyTile);
                EntityManager.SetExecutor(characters[0]);
                _notPossibleTarget.Clear();
            }
            else
            {
                TurnManager.NextTurn();
            }
        }
    }
}
