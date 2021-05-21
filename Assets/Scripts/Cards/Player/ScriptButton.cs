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
    private bool zooming = false;
    private float clickTimer = 0;
    private bool clicked = false;
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
    private void SpawnClick()
    {
        if (selfCard is Unit)
            _buttonCard?.Invoke(selfCard as Unit); //el primero es la sprite de la carta (la morphologia) y el segundo es el boton que contiene esta script.
        else
            spellButton?.Invoke(selfCard as Spell);
    }
    private void Zoom()
    {
        silbingIndex = transform.GetSiblingIndex();

        if (explanation)
            explanation.gameObject.SetActive(true);
        
        transform.SetParent(newParent);
        rect.position += new Vector3(0, offsetY, 0);
        rect.localScale = scale;
        zooming = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isIn && TurnManager.TeamTurn == Team.TeamPlayer)
        {
            clicked = true;
        }

        if (clicked)
        {
            clickTimer += Time.deltaTime;
        }

        HandleZooming();

        if (clickTimer > 0.125 && !dragStart)
        {
            HandleDrag();
        }

        if (holding)
            Dragging();

        if (Input.GetMouseButtonUp(0) && holding)
        {
            holding = false;
            clicked = false;
            dragStart = false;
            clickTimer = 0;
            EndDrag();
        }
        //if (Input.GetMouseButtonDown(0) && isIn)
        //{
        //    if (!dragStart)
        //    {
        //        dragStart = true;
        //        if (!EnoughWhiskas(selfCard))
        //        {
        //            StartCoroutine(NotEnough(im));
        //        }
        //        else
        //        {
        //            holding = true;
        //            StartDrag();
        //        }
        //    }
        //}
        //if (Input.GetMouseButtonDown(0) && !isIn)
        //{
        //    Cancel();
        //}

        //if (holding)
        //    Dragging();
        //else
        //    EndDrag();

        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (isIn && !zooming)
        //    {
        //        Zoom();
        //    }
        //    else if (isIn && !zooming)
        //    {
        //        Cancel();
        //    }
        //    else if (holding)
        //    {
        //        holding = false;
        //        EndDrag();
        //    }
        //}

        //if (currentDisplayer)
        //{
        //    currentDisplayer.transform.position = Input.mousePosition;
        //}

        //clickTimer += Time.deltaTime;
    }

    private void HandleZooming()
    {
        if (Input.GetMouseButtonUp(0) && clickTimer <= 0.2 && clicked && !zooming)
        {
            Zoom();
            clicked = false;
            clickTimer = 0;
        }
        if (Input.GetMouseButtonDown(0) && zooming)
        {
            if (isIn && !dragStart)
            {
                HandleDrag();
            } 
            else if (!isIn)
            {
                Cancel();
            }
        }
    }
    private void HandleDrag()
    {
        if (!EnoughWhiskas(selfCard))
        {
            StartCoroutine(NotEnough(im));
            clicked = false;
            holding = false;
            dragStart = false;
            clickTimer = 0;
        }
        else
        {
            dragStart = true;
            holding = true;
            StartDrag();
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

        zooming = false;
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
