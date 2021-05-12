using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    private Spell currentSpell;
    public HandManager hand;
    private void OnEnable()
    {
        ScriptButton.spellButton += SetSpellActive;
        ScriptButton.endDrag += Execute;
    }
    private void OnDisable()
    {
        ScriptButton.spellButton -= SetSpellActive;
        ScriptButton.endDrag -= Execute;
    }
    private void SetSpellActive(Spell spell)
    {
        currentSpell = spell;
    }
    private void Execute()
    {
        if (currentSpell)
        {
            currentSpell.ExecuteSpell();
            if (currentSpell.executed)
            {
                DestroyCard(currentSpell);
            }
            currentSpell = null;
        }

    }
    public void DestroyCard(Card c)
    {
        TurnManager.SubstractMana(c.Whiskas);
        hand.RemoveCard(c);
        Destroy(c.gameObject);
    }
}
