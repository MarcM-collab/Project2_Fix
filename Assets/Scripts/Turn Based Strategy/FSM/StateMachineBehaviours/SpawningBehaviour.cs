using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningBehaviour : StateMachineBehaviour
{
    public delegate void SpawningEnterDelegate();
    public static SpawningEnterDelegate OnSpawningEnter;
    public delegate void SpawningUpdateDelegate(Animator animator);
    public static SpawningUpdateDelegate OnSpawningUpdate;
    public delegate void SpawningExitDelegate();
    public static SpawningExitDelegate OnSpawningExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnSpawningEnter?.Invoke();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnSpawningUpdate?.Invoke(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnSpawningExit?.Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
