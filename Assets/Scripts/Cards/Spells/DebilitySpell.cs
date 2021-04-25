using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebilitySpell : Spells
{
    private bool executing = false;

    public override void ExecuteSpell()
    {
        executing = true;
    }
    private void Update()
    {
        if (executing)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);
                print(hit2D.collider.gameObject.GetComponent<Character>());
                if (hit2D.collider.gameObject.GetComponent<Character>() == null)//avoids reference error when not clicking a character
                    return;

                Character target = hit2D.collider.gameObject.GetComponent<Character>();
                if (target != null)
                {
                    target.AttackPoints /= 2;
                    print(target.AttackPoints);
                }
                else
                {
                    executing = false;
                }
            }
        }
    }
}
