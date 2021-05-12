using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHideAttackRangeBehaviour : StateMachineBehaviour
{
    public delegate void Melee_HideAttackRangeDelegate();
    public static Melee_HideAttackRangeDelegate OnMeleeHideAttackRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeHideAttackRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
