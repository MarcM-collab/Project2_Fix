using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Melee,
    Ranged,
}
public class Character : Entity
{
    public Class Class;
    public int AttackPoints;
    public int Range;

    [Header("Animation")]
    public float Velocity;

    public bool Teleporting;
    public bool Turn;
    public bool Attack;

    public bool IsExhaustedAnim;

    public Vector3 TeleportPoint;

    [HideInInspector] public int currentAttack;
    private void Start()
    {
        currentAttack = AttackPoints;
        _animator = GetComponent<Animator>();
        HP = MaxHP;
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    private void Update()
    {
        _animator.SetBool("Exhausted", Exhausted);
        _animator.SetBool("Teleporting", Teleporting);
        _animator.SetBool("Turn", Turn);
        _animator.SetBool("Hit", Hit);
        _animator.SetBool("Dead", Dead);
        _animator.SetBool("Attack", Attack);
    }
    public void TurningExecutor(float deltaX)
    {
        if (deltaX < 0)
        {
            if (transform.rotation.eulerAngles.y == 180)
            {
                Turn = true;
            }
        }
        if (deltaX > 0)
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                Turn = true;
            }
        }
    }
    public static bool operator ==(Character e1, Character e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return positionE1.x == positionE2.x && positionE1.y == positionE2.y;
    }
    public static bool operator !=(Character e1, Character e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return !(positionE1.x == positionE2.x && positionE1.y == positionE2.y);
    }
}
