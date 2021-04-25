using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth = 100;

    private float BaseDefense;
    private float RoundDefense;
    public float Defense => BaseDefense + RoundDefense;

    private float BaseAttack;
    private float RoundAttack;
    public float Attack => BaseAttack + RoundAttack;

    private float BaseSpeed;
    private float RoundSpeed;
    public float Speed => BaseSpeed + RoundSpeed;

    public List<FightCommandTypes> PossibleCommands;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    

    public void TakeDamage(float damage)
    {
        float realDamage = damage - (BaseDefense + RoundDefense);
        realDamage = Mathf.Max(realDamage, 0);

        CurrentHealth -= realDamage;

        if (CurrentHealth < 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void AddDefence(float amount)
    {
        RoundDefense += amount;
    }

    public void AddAttack(float amount)
    {
        RoundAttack += amount;
    }

    public void AddSpeed(float amount)
    {
        RoundSpeed += amount;
    }

    public void Reset()
    {
        RoundDefense = 0;
        RoundAttack = 0;
        RoundSpeed = 0;
    }
}
