using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRangeBehaviour : StateMachineBehaviour
{
    public delegate void HideRangeEnterDelegate(Animator animator);
    public static HideRangeEnterDelegate OnHideRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnHideRangeEnter?.Invoke(animator);
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
