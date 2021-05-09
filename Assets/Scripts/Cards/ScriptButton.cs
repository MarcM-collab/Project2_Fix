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
    private int clicked = 0;
    public float offsetY = 300;
    public Vector3 scale;
    private RectTransform rect;
    private Vector3 startScale;
    private float startY;
    private Card selfCard;
    private Transform startParent;
    private Transform newParent;
    private int silbingIndex = 0;
    private void Start()
    {
        silbingIndex = transform.GetSiblingIndex();
        startParent = transform.parent;
        newParent = GetComponentInParent<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        startY = rect.position.y;
        startScale = rect.localScale;
        selfCard = GetComponent<Card>();
    }
    public void ClicButton()
    {
        if (clicked == 0)    //here it will make a zoom
        {
            Zoom();
        }
        else if (clicked == 1)       //here it will make the spawn
        {
            clicked++;
            SpawnClick();
        }
        else
        {
            clicked = 0;
            Cancel();
        }
    }
    private void SpawnClick()
    {
        if (selfCard is Unit)
            _buttonCard?.Invoke(selfCard as Unit); //el primero es la sprite de la carta (la morphologia) y el segundo es el boton que contiene esta script.
        else
            spellButton?.Invoke(selfCard as Spell);
    }

    private void Zoom()
    {
        if (!EnoughWhiskas(selfCard))
        {
            StartCoroutine(NotEnough(selfCard.GetComponent<Image>()));
        }
        else
        {
            clicked++;
            transform.SetParent(newParent);
            rect.position += new Vector3(0, offsetY, 0);
            rect.localScale = scale;
        }
    }
    private void Cancel()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(silbingIndex);
        rect.position = new Vector3(rect.position.x, startY, rect.position.z);
        rect.localScale = startScale;
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
