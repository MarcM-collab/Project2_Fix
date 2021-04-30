using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePathBehaviour : StateMachineBehaviour
{
    public delegate void HidePathEnterDelegate();
    public static HidePathEnterDelegate OnHidePathEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnHidePathEnter?.Invoke();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
