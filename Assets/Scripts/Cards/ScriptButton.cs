using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ScriptButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    //script para que los botones pasaran correctamente así mismo y prefabs.
    public delegate void ButtonCard(Unit card); 
    public static ButtonCard _buttonCard;

    public delegate void ClickedSpell(Spell card);
    public static ClickedSpell spellButton;

    public delegate void DragEnded();
    public static DragEnded endDrag;

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
    private Explanation explanation;
    private bool isIn = false;
    private Image im;
    [Range(0, 1)] private float transparencyOnDrag = 0.5f;
    public GameObject displayer;
    private GameObject currentDisplayer;
    private bool holding = false;
    private bool dragStart = false;
    private void Start()
    {
        im = GetComponent<Image>();
        explanation = GetComponentInChildren<Explanation>();
        if (explanation)
            explanation.gameObject.SetActive(false);

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
        else if (clicked > 0)       //here it will make the spawn
        {

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
        if (explanation)
            explanation.gameObject.SetActive(true);

        clicked++;
        transform.SetParent(newParent);
        rect.position += new Vector3(0, offsetY, 0);
        rect.localScale = scale;
    }
    private void Update()
    {
        if (clicked >= 1 && Input.GetMouseButtonDown(0) &&!isIn)
        {
            Cancel();
        }
        else if (clicked > 0 && Input.GetMouseButtonDown(0))
        {
            if (!dragStart)
            {
                dragStart = true;
                if (!EnoughWhiskas(selfCard))
                {
                    StartCoroutine(NotEnough(im));
                }
                else
                {
                    holding = true;
                    StartDrag();
                }
            }
        }

        if (holding)
            Dragging();

        if (Input.GetMouseButtonUp(0))
        {
            if (holding)
                EndDrag();

            holding = false;
        }


        if (currentDisplayer)
        {
            currentDisplayer.transform.position = Input.mousePosition;
        }
    }

    private void Cancel()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(silbingIndex);
        rect.position = new Vector3(rect.position.x, startY, rect.position.z);
        rect.localScale = startScale;

        if (explanation)
            explanation.gameObject.SetActive(false);

        clicked = 0;
    }
    private IEnumerator NotEnough(Image i)
    {
        i.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        i.color = Color.white;
        dragStart = false;
    }
    private bool EnoughWhiskas(Card _card)//comprueba si hay whiskas (maná) suficiente para lanzar la carta
    {
        return _card.Whiskas <= TurnManager.currentMana;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
    }
    private void StartDrag()
    {
        im.color = new Color(im.color.r, im.color.g, im.color.b, transparencyOnDrag);
        clicked++;
        SpawnClick();
        currentDisplayer = Instantiate(displayer, Input.mousePosition, Quaternion.identity);
        currentDisplayer.GetComponent<Image>().sprite = im.sprite;
    }
    private void Dragging()
    {
        if (currentDisplayer)
            currentDisplayer.transform.position = Input.mousePosition;
    }
    private void EndDrag()
    {
        endDrag?.Invoke();
        dragStart = false;
        im.color = new Color(im.color.r, im.color.g, im.color.b, 1);
        Destroy(currentDisplayer);
    }
}
