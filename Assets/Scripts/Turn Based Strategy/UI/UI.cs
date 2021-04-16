using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI
{
    public delegate void ShowManaText(int currentMana, int maxMana);
    public static ShowManaText OnShowManaText;
    public static void ManaText(int currentMana, int maxMana)
    {
        OnShowManaText?.Invoke(currentMana, maxMana);
    }
}

