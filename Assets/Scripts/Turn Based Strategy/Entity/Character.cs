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
    [Header("Stats")]
    public Class Class;
    public int AttackPoints;
    public int Health;
    public int Range;

    [Header("Animation")]
    public float Velocity;

    public bool Walking;
    public bool Turn;
    public bool Attack;

    public bool IsExhaustedAnim;

    public List<Vector3> Path;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        MaxHP= HP;
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    private void Update()
    {
        _animator.SetBool("Exhausted", Exhausted);
        _animator.SetBool("Walking", Walking);
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
