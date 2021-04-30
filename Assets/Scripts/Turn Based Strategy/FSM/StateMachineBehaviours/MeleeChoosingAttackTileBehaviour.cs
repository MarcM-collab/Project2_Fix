using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChoosingAttackTileBehaviour : StateMachineBehaviour
{
    public delegate void Melee_ChoosingAttackTileEnterDelegate(Animator animator);
    public static Melee_ChoosingAttackTileEnterDelegate OnMeleeChoosingAttackTileEnter;
    public delegate void Melee_ChoosingAttackTileUpdateDelegate(Animator animator);
    public static Melee_ChoosingAttackTileUpdateDelegate OnMeleeChoosingAttackTileUpdate;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeChoosingAttackTileEnter?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnMeleeChoosingAttackTileUpdate?.Invoke(animator);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
}
