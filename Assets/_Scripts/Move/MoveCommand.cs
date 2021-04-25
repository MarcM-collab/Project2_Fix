using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    private Vector2 _direction;

    public MoveCommand(Entity entity, Vector2 direction) : base(entity)
    {
        _direction = direction;
    }
    public override void Excecute()
    {
        _entity.transform.Translate(_direction);
    }

    public override void Undo()
    {
        _entity.transform.Translate(-_direction);
    }

   
}
