using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private int maxMana = 1;
    private int currentTurn = 0;

    public int currentMana { get; private set; } = 0;

    public delegate void SetDisplayValue(float value, int currentAmount);
    public static SetDisplayValue setDisplay;

    public GameObject AI;
    public GameObject Player;
    public bool PlayerTurn
    {
        get { return currentTurn % 2 != 0; }
    }
    public bool IsAttackRound;
    void Start()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        currentTurn++;

        if (PlayerTurn)
            maxMana++;

        currentMana = maxMana;
        setDisplay?.Invoke((float)currentMana / maxMana, currentMana);

        //EntityManager.SwapTeams();
        SetTeams();
        EntityManager.TurnChaged = true;
        EntityManager.RemoveExhaust();
    }
    private void SetTeams()
    {
        if (PlayerTurn)
        {
            AI.SetActive(false);
            Player.SetActive(true);
        }
        else
        {
            AI.SetActive(true);
            Player.SetActive(false);
        }
    }

    public void SubstractMana(int amount)
    {
        currentMana -= amount;
        setDisplay?.Invoke((float)currentMana / maxMana, currentMana);
    }
}
