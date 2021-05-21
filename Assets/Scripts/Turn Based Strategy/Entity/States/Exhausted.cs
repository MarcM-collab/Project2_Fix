using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhausted : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentsInChildren<SpriteRenderer>()[1].color = Color.gray;
        animator.GetComponent<Character>().IsExhaustedAnim = true;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
        animator.GetComponent<Character>().IsExhaustedAnim = false;
    }
}
