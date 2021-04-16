using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command, IDealDamage
{
    //public bool HasDied;
    //public float DeniedDamage;
    public AttackCommand(Entity target, Entity executor) : base(target, executor)
    {
        Invoker.AddCommand(this);
    }
    public override void Excecute()
    {
        DealDamage();
    }
    public void DealDamage()
    {
        HealthSystem.TakeDamage(_executor.AttackPoints);
    }
}
