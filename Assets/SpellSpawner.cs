using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    private Spell currentSpell;
    private void OnEnable()
    {
        ButtonSpell.spellButton += SetSpellActive;
    }
    private void OnDisable()
    {
        ButtonSpell.spellButton -= SetSpellActive;
    }
    private void SetSpellActive(Spell spell)
    {
        currentSpell = spell;
    }
    private void Update()
    {
        if (currentSpell)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentSpell.CanBeUsed())
                {
                    currentSpell.ExecuteSpell();
                    TurnManager.SubstractMana(currentSpell.Whiskas);
                    Destroy(currentSpell.gameObject);
                }
                currentSpell = null;
            }

        }

    }
}
