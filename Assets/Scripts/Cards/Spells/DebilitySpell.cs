using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebilitySpell : Spells
{
    private bool executing = false;
    private bool used = false; //temporal
    public override void ExecuteSpell()
    {
        executing = true;
    }
    private void Update()
    {
        if (executing)
        {
            if (Input.GetMouseButton(0) &&!used)
            {
                RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);

                if (hit2D)
                {

                    if (hit2D.transform.CompareTag("Character"))
                    {
                        used = true;
                        Character target = hit2D.collider.gameObject.GetComponent<Character>();
                        target.AttackPoints /= 2;

                        if (target.AttackPoints <= 0)
                            target.AttackPoints = 1;

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
}
