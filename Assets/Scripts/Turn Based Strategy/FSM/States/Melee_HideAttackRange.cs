using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_HideAttackRange : StateMachineBehaviour
{
    public delegate void Melee_HideAttackRangeDelegate();
    public static Melee_HideAttackRangeDelegate OnMelee_HideAttackRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMelee_HideAttackRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
