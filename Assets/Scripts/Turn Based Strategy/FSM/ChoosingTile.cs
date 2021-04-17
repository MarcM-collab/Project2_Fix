using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingTile: StateMachineBehaviour
{
    public delegate void ChoosingTileUpdateDelegate(Animator animator);
    public static ChoosingTileUpdateDelegate OnChoosingTileUpdate;
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnChoosingTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
