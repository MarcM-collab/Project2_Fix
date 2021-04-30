using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedShowRangeBehaviour : StateMachineBehaviour
{
    public delegate void Ranged_ShowRangeEnterDelegate();
    public static Ranged_ShowRangeEnterDelegate OnRangedShowRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnRangedShowRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
