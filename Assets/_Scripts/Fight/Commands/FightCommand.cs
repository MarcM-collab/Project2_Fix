using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public abstract class FightCommand : ICommand
{
    protected Fighter _executor;
    protected Fighter _target;
    protected float _priority;

   

    protected FightCommand(Fighter executor, Fighter target, float priority)
    {
        _executor = executor;
        _target = target;
        _priority = priority;
    }

    public abstract void Excecute();
    public abstract void Undo();

   
}


public enum FightCommandTypes
{
    Attack,
    Defend
}