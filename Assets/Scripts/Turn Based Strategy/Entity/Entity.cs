using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    TeamA,
    TeamB,
}
public enum Class
{
    Melee,
    Ranged,
}
public class Entity : MonoBehaviour
{
    public delegate void ChangeHealthDelegate(float fractionHealth);
    public ChangeHealthDelegate OnChangeHealth;

    [Header("Stats")]
    public Class Class;
    public float HP;
    public int AttackPoints;
    private float MaxHP;
    public int Range;

    [Header("Animation")]
    public float Velocity;

    [Header("Other")]
    public Team Team;
    public bool IsAlive => HP > 0;
    public bool IsActive => IsAlive && !Exhausted;

    public bool Exhausted;
    public bool Walking;
    public bool Turn;
    public List<Vector3> Path;


    private Animator _animator;

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
    }
    public void ChangeHealth()
    {
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    public static bool operator ==(Entity e1, Entity e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return positionE1.x == positionE2.x && positionE1.y == positionE2.y;
    }
    public static bool operator !=(Entity e1, Entity e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return !(positionE1.x == positionE2.x && positionE1.y == positionE2.y);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }
    public void Hit()
    {
        _animator.SetTrigger("Hit");
    }
}
