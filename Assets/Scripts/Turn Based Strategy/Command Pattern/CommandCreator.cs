using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandCreator
{
    private static Entity _target => EntityManager.TargetEntity;
    private static Entity _executor => EntityManager.ExecutorEntity;

    public static AttackCommand CurrentAttackCommand;

    public static void Attack()
    {
        var command = new AttackCommand(_target, _executor);
        CurrentAttackCommand = command;
    }
}
