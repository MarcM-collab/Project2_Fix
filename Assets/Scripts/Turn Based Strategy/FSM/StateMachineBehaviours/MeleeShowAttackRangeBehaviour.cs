using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShowAttackRangeBehaviour : StateMachineBehaviour
{
    public delegate void Melee_ShowAttackRangeDelegate();
    public static Melee_ShowAttackRangeDelegate OnMeleeShowAttackRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeShowAttackRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
