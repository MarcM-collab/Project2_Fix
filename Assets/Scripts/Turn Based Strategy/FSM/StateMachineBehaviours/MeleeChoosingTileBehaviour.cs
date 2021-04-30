using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingTileBehaviour: StateMachineBehaviour
{
    public delegate void Melee_ChoosingTileEnterDelegate(Animator animator);
    public static Melee_ChoosingTileEnterDelegate OnMeleeChoosingTileEnter;
    public delegate void Melee_ChoosingTileUpdateDelegate(Animator animator);
    public static Melee_ChoosingTileUpdateDelegate OnMeleeChoosingTileUpdate;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeChoosingTileEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeChoosingTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
