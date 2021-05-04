using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhausting : CombatBehaviour
{
    private bool _targetIsDead;
    private void OnEnable()
    {
        ExhaustingBehaviour.OnExhaustingEnter += ExhaustingEnter;
        ExhaustingBehaviour.OnExhaustingUpdate += ExhaustingUpdate;
        ExhaustingBehaviour.OnExhaustingExit += ExhaustingExit;
    }
    private void OnDisable()
    {
        ExhaustingBehaviour.OnExhaustingEnter -= ExhaustingEnter;
        ExhaustingBehaviour.OnExhaustingUpdate -= ExhaustingUpdate;
        ExhaustingBehaviour.OnExhaustingExit -= ExhaustingExit;
    }
    private void ExhaustingEnter(Animator animator)
    {
        if (!_targetEntity.IsAlive)
        {
            _targetIsDead = true;
        }

        ResetAnimatorParamaters(animator);

        _executorCharacter.Exhausted = true;

        Cursor.SetCursor(_cursorHand, Vector2.zero, CursorMode.Auto);
    }
    private void ResetAnimatorParamaters(Animator animator)
    {
        foreach (AnimatorControllerParameter paramater in animator.parameters)
        {
            if (paramater.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(paramater.name, false);
            }
        }
    }
    private void ExhaustingUpdate(Animator animator)
    {
        if (_executorCharacter.IsExhaustedAnim)
        {
            if (!_targetIsDead)
            {
                animator.SetTrigger("Exhausted");
            }
            else
            {
                if (!_targetEntity.IsActive)
                {
                    animator.SetTrigger("Exhausted");
                    _targetIsDead = false;
                }
            }
        }
    }
    private void ExhaustingExit(Animator animator)
    {
    }
}
