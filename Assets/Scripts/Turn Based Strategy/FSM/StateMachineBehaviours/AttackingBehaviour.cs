using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingBehaviour : StateMachineBehaviour
{
    public delegate void AttackingEnterDelegate();
    public static AttackingEnterDelegate OnAttackingEnter;
    public delegate void AttackingUpdateDelegate(Animator animator);
    public static AttackingUpdateDelegate OnAttackingUpdate;
    public delegate void AttackingExitDelegate();
    public static AttackingExitDelegate OnAttackingExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAttackingEnter?.Invoke();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAttackingUpdate?.Invoke(animator);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAttackingExit?.Invoke();
    }
}
