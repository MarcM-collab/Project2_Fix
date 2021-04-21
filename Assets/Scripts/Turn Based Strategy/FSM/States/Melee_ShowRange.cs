using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_ShowRange : StateMachineBehaviour
{
    public delegate void Melee_ShowRangeEnterDelegate();
    public static Melee_ShowRangeEnterDelegate OnMelee_ShowRangeEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMelee_ShowRangeEnter?.Invoke();
    }
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
