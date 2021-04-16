using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRange : StateMachineBehaviour
{
    public delegate void ShowRangeEnterDelegate();
    public static ShowRangeEnterDelegate OnShowRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnShowRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
