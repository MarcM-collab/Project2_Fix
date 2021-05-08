using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : Entity
{
    private bool Cast;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        HP = MaxHP;
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    private void Update()
    {
        _animator.SetBool("Exhausted", Exhausted);
        _animator.SetBool("Hit", Hit);
        _animator.SetBool("Dead", Dead);
        _animator.SetBool("Cast", Cast);

        if (Dead)
        {
            if (Team == Team.TeamAI)
                SceneManager.LoadScene("EndPlayer");
            else
                SceneManager.LoadScene("EndLoss");
        }
    }
}
