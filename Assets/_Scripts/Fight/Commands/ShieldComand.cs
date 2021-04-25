using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComand : FightCommand
{
    protected static float _priorityBonus;
    protected float _protectionAmount;
    public ShieldComand(Fighter executor, Fighter target, float priority, float protection) : base(executor, target, priority)
    {
        _priority += _priorityBonus;
        _protectionAmount = protection;
    }

    public override void Excecute()
    {
        _target.AddDefence(_protectionAmount);
    }

    public override void Undo()
    {
       
    }
}
