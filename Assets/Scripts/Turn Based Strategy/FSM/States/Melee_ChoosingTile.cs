using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_ChoosingTile: StateMachineBehaviour
{
    public delegate void Melee_ChoosingTileUpdateDelegate(Animator animator);
    public static Melee_ChoosingTileUpdateDelegate OnMelee_ChoosingTileUpdate;
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMelee_ChoosingTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
