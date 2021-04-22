using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecting : StateMachineBehaviour
{
    public delegate void SelectingUpdateDelegate(Animator animator);
    public static SelectingUpdateDelegate OnSelectingUpdate;
    public delegate void SelectingExitDelegate();
    public static SelectingExitDelegate OnSelectingExit;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnSelectingUpdate?.Invoke(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnSelectingExit?.Invoke();
    }
}
