using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustingBehaviour : StateMachineBehaviour
{
    public delegate void ExhaustingEnterDelegate(Animator animator);
    public static ExhaustingEnterDelegate OnExhaustingEnter;
    public delegate void ExhaustingUpdateDelegate(Animator animator);
    public static ExhaustingUpdateDelegate OnExhaustingUpdate;
    public delegate void ExhaustingExitDelegate(Animator animator);
    public static ExhaustingExitDelegate OnExhaustingExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingUpdate?.Invoke(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExhaustingExit?.Invoke(animator);
    }
}
