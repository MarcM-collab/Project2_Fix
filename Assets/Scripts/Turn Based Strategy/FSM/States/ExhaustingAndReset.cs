using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustingAndReset : StateMachineBehaviour
{
    public delegate void ExhaustingAndResetEnterDelegate(Animator animator);
    public static ExhaustingAndResetEnterDelegate OnExhaustingAndResetEnter;
    public delegate void ExhaustingAndResetUpdateDelegate(Animator animator);
    public static ExhaustingAndResetUpdateDelegate OnExhaustingAndResetUpdate;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingAndResetEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingAndResetUpdate?.Invoke(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
