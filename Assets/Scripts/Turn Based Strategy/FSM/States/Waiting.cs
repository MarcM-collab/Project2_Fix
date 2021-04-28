using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting : StateMachineBehaviour
{
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TurnManager turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        if (CardUsage.isDragging)
        {
            animator.SetBool("IsDragging", true);
        }
        Team teamPlaying = turnManager.PlayerTurn ? Team.TeamPlayer : Team.TeamAI;
        if (EntityManager.GetActiveCharacters(teamPlaying).Length > 0)
        {
            if (teamPlaying == Team.TeamAI && IACard.StartAI)
            {
                animator.SetBool("NotWaiting", true);
            }
            else if (teamPlaying == Team.TeamPlayer && turnManager.IsAttackRound)
            {
                animator.SetBool("NotWaiting", true);
            }
        }
        else if (IACard.StartAI)
        {
            turnManager.NextTurn();
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
