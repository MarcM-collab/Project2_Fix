using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_ShowAttackRange : StateMachineBehaviour
{
    public delegate void Melee_ShowAttackRangeDelegate();
    public static Melee_ShowAttackRangeDelegate OnMelee_ShowAttackRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMelee_ShowAttackRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
