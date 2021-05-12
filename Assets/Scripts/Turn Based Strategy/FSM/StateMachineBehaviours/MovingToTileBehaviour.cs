using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTileBehaviour : StateMachineBehaviour
{
    public delegate void MovingToTileEnterDelegate();
    public static MovingToTileEnterDelegate OnMovingToTileEnter;
    public delegate void MovingToTileUpdateDelegate(Animator animator);
    public static MovingToTileUpdateDelegate OnMovingToTileUpdate;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMovingToTileEnter?.Invoke();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMovingToTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
