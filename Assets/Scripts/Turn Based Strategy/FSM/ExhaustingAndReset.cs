using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustingAndReset : StateMachineBehaviour
{
    public delegate void ExhaustingAndResetEnterDelegate(Animator animator);
    public static ExhaustingAndResetEnterDelegate OnExhaustingAndResetEnter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingAndResetEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
