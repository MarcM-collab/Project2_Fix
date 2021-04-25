using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HealthSystem.TakeDamage(CharacterManager.ExecutorCharacter.AttackPoints);
        animator.GetComponent<Character>().ChangeHealth();
        animator.GetComponent<SpriteRenderer>().color = Color.red;
        animator.GetComponent<Character>().Hit = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
