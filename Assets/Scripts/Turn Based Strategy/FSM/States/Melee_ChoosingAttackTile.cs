using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_ChoosingAttackTile : StateMachineBehaviour
{
    public delegate void Melee_ChoosingAttackTileUpdateDelegate(Animator animator);
    public static Melee_ChoosingAttackTileUpdateDelegate OnMelee_ChoosingAttackTileUpdate;
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMelee_ChoosingAttackTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
