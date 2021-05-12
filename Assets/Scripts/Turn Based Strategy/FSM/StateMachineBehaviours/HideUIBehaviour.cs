using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIBehaviour : StateMachineBehaviour
{
    public delegate void HideUIEnterDelegate();
    public static HideUIEnterDelegate OnHideUIEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnHideUIEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
