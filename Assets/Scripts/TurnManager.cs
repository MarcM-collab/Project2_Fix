using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TurnManager
{
    private static int maxMana = 1;
    private static int currentTurn = 0;

    public static int currentMana { get; private set; } = 0;

    public delegate void SetDisplayValue(float value, int currentAmount);
    public static SetDisplayValue setDisplay;

    public delegate void SwitchBehaviour();
    public static SwitchBehaviour OnSwitchBehaviour;
    public static Team TeamTurn
    {
        get 
        {
            if (currentTurn % 2 != 0)
            {
                return Team.TeamPlayer;
            }
            return Team.TeamAI;
        }
    }
    public static bool IsAttackRound;

    [RuntimeInitializeOnLoadMethod]
    public static void NextTurn()
    {
        currentTurn++;

        if (TeamTurn == Team.TeamPlayer)
            maxMana++;

        currentMana = maxMana;
        setDisplay?.Invoke((float)currentMana / maxMana, currentMana);

        OnSwitchBehaviour?.Invoke();
        EntityManager.TurnChaged = true;
        EntityManager.RemoveExhaust();
    }

    public static void SubstractMana(int amount)
    {
        currentMana -= amount;
        setDisplay?.Invoke((float)currentMana / maxMana, currentMana);
    }
}
