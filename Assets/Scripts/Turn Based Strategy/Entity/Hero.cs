using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    private bool Cast;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        MaxHP = HP;
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    private void Update()
    {
        _animator.SetBool("Exhausted", Exhausted);
        _animator.SetBool("Hit", Hit);
        _animator.SetBool("Dead", Dead);
        _animator.SetBool("Cast", Cast);
    }
}
