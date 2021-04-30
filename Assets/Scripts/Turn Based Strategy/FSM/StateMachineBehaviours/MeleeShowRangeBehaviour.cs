using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShowRangeBehaviour : StateMachineBehaviour
{
    public delegate void Melee_ShowRangeEnterDelegate();
    public static Melee_ShowRangeEnterDelegate OnMeleeShowRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeShowRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
