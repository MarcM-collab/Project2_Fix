using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptButton : MonoBehaviour
{
    //script para que los botones pasaran correctamente así mismo y prefabs.
    public delegate void ButtonCard(Unit card); 
    public static ButtonCard _buttonCard;

    public delegate void ClickedSpell(Spell card);
    public static ClickedSpell spellButton;
    public void ClicButton()
    {
        Card selfCard = GetComponent<Card>();
        if (!EnoughWhiskas(selfCard))
        {
            StartCoroutine(NotEnough(selfCard.GetComponent<Image>()));
        }
        else
        {
            if (selfCard is Unit)
                _buttonCard?.Invoke(selfCard as Unit); //el primero es la sprite de la carta (la morphologia) y el segundo es el boton que contiene esta script.
            else
                spellButton?.Invoke(selfCard as Spell);
        }

    }
    private IEnumerator NotEnough(Image i)
    {
        i.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        i.color = Color.white;
    }
    private bool EnoughWhiskas(Card _card)//comprueba si hay whiskas (maná) suficiente para lanzar la carta
    {
        return _card.Whiskas <= TurnManager.currentMana;
    }
}
