using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedChoosingTileBehaviour : StateMachineBehaviour
{
    public delegate void Ranged_ChoosingTileEnterDelegate(Animator animator);
    public static Ranged_ChoosingTileEnterDelegate OnRangedChoosingTileEnter;
    public delegate void Ranged_ChoosingTileUpdateDelegate(Animator animator);
    public static Ranged_ChoosingTileUpdateDelegate OnRangedChoosingTileUpdate;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnRangedChoosingTileEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnRangedChoosingTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
