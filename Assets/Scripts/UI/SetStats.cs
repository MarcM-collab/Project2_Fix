using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetStats : MonoBehaviour
{
    public TMP_Text health, attack, cost, range;
    private Card c;
    private void Awake()
    {
        c = GetComponent<Card>();
        
        if (c)
        {
            SetTexts();
        }
    }
    private void SetTexts()
    {
        cost.text = c.Whiskas.ToString();
        if (c is Unit)
        {
            Unit cU = c as Unit;
            health.text = cU.character.MaxHP.ToString();
            attack.text = cU.character.AttackPoints.ToString();
            range.text = cU.character.Range.ToString();
        }
    }
}
