using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingBehaviour : StateMachineBehaviour
{
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CardUsage.isDragging)
        {
            animator.SetBool("IsDragging", true);
        }
        if (EntityManager.GetActiveCharacters(TurnManager.TeamTurn).Length > 0)
        {
            if (TurnManager.TeamTurn == Team.TeamAI && IACard.StartAI)
            {
                animator.SetBool("NotWaiting", true);
            }
            else if (TurnManager.TeamTurn == Team.TeamPlayer && TurnManager.IsAttackRound)
            {
                animator.SetBool("NotWaiting", true);
            }
        }
        else if (IACard.StartAI)
        {
            TurnManager.NextTurn();
            IACard.StartAI = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
