using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var entity = CharacterManager.ExecutorCharacter;
        Vector3 direction = (entity.Path[1] - entity.Path[0]);
        direction = direction.normalized;
        if (entity.transform.position != entity.Path[1])
        {
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, entity.Path[1], entity.Velocity * Time.deltaTime);
        }
        else
            entity.Walking = false;

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
