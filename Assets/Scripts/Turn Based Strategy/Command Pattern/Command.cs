using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : ICommand
{
    protected Entity _target;
    protected Entity _executor;

    protected Command(Entity target, Entity executor)
    {
        _target = target;
        _executor = executor;
    }

    public abstract void Excecute();
}