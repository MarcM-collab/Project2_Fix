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
        ScriptButton.endDrag += ExecutePlayer;
    }
    private void OnDisable()
    {
        ScriptButton.spellButton -= SetSpellActive;
        ScriptButton.endDrag -= ExecutePlayer;
    }
    public void IASpawn(Spell spell)
    {
        currentSpell = spell;
        Execute(Team.TeamAI);
    }
    private void SetSpellActive(Spell spell)
    {
        currentSpell = spell;
    }
    private void ExecutePlayer()
    {
        Execute(Team.TeamPlayer);
    }
    private void Execute(Team team)
    {
        if (currentSpell)
        {
            print("Executing");
            switch (team)
            {
                case Team.TeamPlayer:
                    currentSpell.ExecuteSpell();
                    if (currentSpell.executed)
                    {
                        DestroyCard(currentSpell);
                    }
                    break;
                case Team.TeamAI:
                    currentSpell.IAUse();
                    break;
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
