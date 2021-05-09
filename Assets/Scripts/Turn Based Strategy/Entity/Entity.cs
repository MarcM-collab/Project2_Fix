using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    TeamPlayer,
    TeamAI,
}
public class Entity : MonoBehaviour
{
    public delegate void ChangeHealthDelegate(float fractionHealth);
    public ChangeHealthDelegate OnChangeHealth;

    [HideInInspector] public float HP;
    public Team Team;



    public bool IsAlive => HP > 0;
    public bool IsActive => IsAlive && !Exhausted;

    public bool Exhausted;
    public bool Hit;
    public bool Dead;

    internal Animator _animator;

    [Header("Stats")]
    public float MaxHP;
    public void ChangeHealth()
    {
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
}
