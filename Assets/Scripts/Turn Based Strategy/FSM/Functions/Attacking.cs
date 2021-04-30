using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : CombatBehaviour
{
    private void OnEnable()
    {
        AttackingBehaviour.OnAttackingEnter += AttackingEnter;
        AttackingBehaviour.OnAttackingUpdate += AttackingUpdate;
        AttackingBehaviour.OnAttackingExit += AttackingExit;
    }
    private void OnDisable()
    {
        AttackingBehaviour.OnAttackingEnter -= AttackingEnter;
        AttackingBehaviour.OnAttackingUpdate -= AttackingUpdate;
        AttackingBehaviour.OnAttackingExit -= AttackingExit;
    }
    private void AttackingEnter()
    {
        var deltaX = _targetGridPosition.x - _tileChosenGridPosition.x;
        _executorCharacter.TurningExecutor(deltaX);
    }
    private void AttackingUpdate(Animator animator)
    {
        animator.SetBool("Attacking", _executorCharacter.Turn);
    }
    private void AttackingExit()
    {
        _executorCharacter.Attack = true;
    }
}
