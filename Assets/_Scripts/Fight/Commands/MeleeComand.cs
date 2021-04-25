using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComand : FightCommand
{
    public MeleeComand(Fighter executor, Fighter target, float priority) : base(executor, target, priority)
    {
    }

    public override void Excecute()
    {
        _target.TakeDamage(_executor.Attack);
    }

    public override void Undo()
    {
       
    }
}
