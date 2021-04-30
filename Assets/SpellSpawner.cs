using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    private TurnManager turn;
    private Spell currentSpell;
    private void OnEnable()
    {
        ButtonSpell.spellButton += SetSpellActive;
    }
    private void OnDisable()
    {
        ButtonSpell.spellButton -= SetSpellActive;
    }
    private void Start()
    {
        turn = FindObjectOfType<TurnManager>();
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
                currentSpell.ExecuteSpell();
                if (currentSpell.executed)
                {
                    turn.SubstractMana(currentSpell.Whiskas);
                    Destroy(currentSpell.gameObject);
                }
                currentSpell = null;
            }

        }

    }
}
