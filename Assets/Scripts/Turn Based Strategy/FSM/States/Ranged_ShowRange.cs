using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_ShowRange : StateMachineBehaviour
{
    public delegate void Ranged_ShowRangeEnterDelegate();
    public static Ranged_ShowRangeEnterDelegate OnRanged_ShowRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnRanged_ShowRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
