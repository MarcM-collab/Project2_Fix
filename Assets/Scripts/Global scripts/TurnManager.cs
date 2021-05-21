using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TurnManager
{
    public static int manaLimit = 9;
    private static int maxMana = 1;
    private static int currentTurn = 0;

    public static int currentMana { get; private set; } = 0;

    public delegate void SetDisplayValue(int currentAmount, int maxMana);
    public static SetDisplayValue setDisplay;

    public delegate void SwitchBehaviour();
    public static SwitchBehaviour OnSwitchBehaviour;

    public static bool CardDrawn = false;
    public static bool Spawned;

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

        if (TeamTurn == Team.TeamPlayer && currentTurn < manaLimit*2-2)
            maxMana++;

        currentMana = maxMana;
        setDisplay?.Invoke(currentMana, maxMana);

        OnSwitchBehaviour?.Invoke();
        CardDrawn = false;
        Spawned = false;
        EntityManager.RemoveExhaust();
    }

    public static void SubstractMana(int amount)
    {
        currentMana -= amount;
        setDisplay?.Invoke(currentMana, maxMana);
    }
}
