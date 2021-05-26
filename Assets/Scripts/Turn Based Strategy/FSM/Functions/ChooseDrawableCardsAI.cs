using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChooseDrawableCardsAI : CardAIBehaviour
{
    [SerializeField]
    private DeckAI _deckAI;

    private float maxCardsInHand = 6;
    public Transform IAHandCanvas;
    private List<CardType> IADeck;//deck hecho por nosotros

    [Header("Display settings")]
    public Sprite cardSprites;
    public Image[] selectableStartCards;
    public float selectWait = 2;
    [Range(0, 1)] public float selectWaitRandomicity = 0.25f;
    private float currentWaitSelect = 0;
    [Range(0, 2)] public float selectCardFrequency = 1;
    private float currentSelectFrequency = 0;
    public Color selectCardColor;
    public float cardUsageWait = 2;
    [Range(0, 1)] public float cardUsageRandomicity = 0.25f;
    public float scale = 1f;

    public static bool StartAI;
    private void OnEnable()
    {
        ChooseDrawableCardsBehaviour.OnChooseDrawableCardsEnter += ChooseDrawableCardsEnter;
        ChooseDrawableCardsBehaviour.OnChooseDrawableCardsUpdate += ChooseDrawableCardsUpdate;
    }
    private void OnDisable()
    {
        ChooseDrawableCardsBehaviour.OnChooseDrawableCardsEnter -= ChooseDrawableCardsEnter;
        ChooseDrawableCardsBehaviour.OnChooseDrawableCardsUpdate -= ChooseDrawableCardsUpdate;
    }

    private void Start()
    {
        IADeck = _deckAI.IADeck;
    }

    private void ChooseDrawableCardsEnter(Animator animator)
    {
        ShowInitialCards();
    }
    private void ShowInitialCards()
    {
        for (int i = 0; i < selectableStartCards.Length; i++)
        {
            selectableStartCards[i].gameObject.SetActive(true);
            selectableStartCards[i].sprite = cardSprites;
        }
    }
    private void ChooseDrawableCardsUpdate(Animator animator)
    {
        if (currentWaitSelect < selectWait && animator.GetBool("ChooseCard"))
        {
            currentWaitSelect += Time.deltaTime;

            if (currentWaitSelect >= currentSelectFrequency + Random.Range(-selectWaitRandomicity, selectWaitRandomicity))
            {
                SetSelectedInitialCard(Random.Range(0, selectableStartCards.Length));
                currentSelectFrequency += selectCardFrequency;
            }
        }
        else if (Mathf.FloorToInt(currentWaitSelect) == Mathf.FloorToInt(selectWait)) //avoids extra executions
        {
            currentWaitSelect++;
            HideInitialCards();
            var IsBelowHandMaxSize = IAHand.Count < maxCardsInHand;
            if (IsBelowHandMaxSize)
                RandomCardChosen();
            TurnManager.CardDrawn = true;
            currentWaitSelect = 0;
            currentSelectFrequency = 0;
            animator.SetBool("ChooseCard", false);
        }
    }
    private void HideInitialCards()
    {
        SetSelectedInitialCard(-1);
        for (int i = 0; i < selectableStartCards.Length; i++)
            selectableStartCards[i].gameObject.SetActive(false); //hide cards
    }
    private void SetSelectedInitialCard(int v)
    {
        for (int i = 0; i < selectableStartCards.Length; i++)
        {
            selectableStartCards[i].color = Color.white;
        }

        if (v != -1) //reset colors
            selectableStartCards[v].color = selectCardColor;
    }
    private void RandomCardChosen()
    {
        int random1 = Random.Range(0, IADeck.Count);
        int random2 = Random.Range(0, IADeck.Count);

        while (random1 == random2)
        {
            random2 = Random.Range(0, IADeck.Count);
        }

        AddCardHand(ComproveHand(random1, random2));
    }
    private void AddCardHand(Card toSpawn)
    {
        var cardInstance = Instantiate(toSpawn, IAHandCanvas.position, Quaternion.identity).transform;
        IAHand.Add(cardInstance.GetComponent<Card>()); //Avoids modifing the prefab
        cardInstance.GetComponent<Button>().enabled = false; //Avoids interaction with player
        cardInstance.GetComponent<ScriptButton>().enabled = false;
        cardInstance.GetComponent<Image>().sprite = cardSprites;

        Transform[] stats = cardInstance.GetComponentsInChildren<Transform>();
        foreach (Transform t in stats)
        {
            if (t != cardInstance.transform)
                t.gameObject.SetActive(false);
        }

        cardInstance.SetParent(IAHandCanvas);
        cardInstance.localScale = new Vector3(scale, scale, scale);//escalamos las cartas que se ven en la mano.
    }
    private Card ComproveHand(int random1, int random2)//comprueba que cartas tiene la IA en su mano.
    {
        // if one and only one of the cards is repited...
        bool _firstCardRepe = false;
        bool _secondCardRepe = false;

        //miramos en la mano cuales tiene.
        for (int i = 0; i < IAHand.Count; i++)
        {
            if (IAHand[i].name == IADeck[random1].card.name) //si el nombre es diferente =>  no la tiene| coge esta y no comprueba las otras.
            {
                _firstCardRepe = true;
            }
            else if (IAHand[i].name == IADeck[random2].card.name) //si el nombre es diferente =>  no la tiene
            {
                _secondCardRepe = true;
            }
        }
        if (!_firstCardRepe && _secondCardRepe)
        {
            return IADeck[random1].card;
        }
        else if (!_secondCardRepe && _firstCardRepe)
        {
            return IADeck[random2].card;
        }
        else //sistema de prioridad.
        {
            if (IADeck[random1].priority <= IADeck[random2].priority) //They shouldn't be equal but if they are it will be chosen randomly (the one on the first spot which was randomly assigned)
                return IADeck[random1].card;
            else
                return IADeck[random2].card;
        }
    }
    private void SetPriorities()
    {
        //List<CardType> tempList = new List<CardType>();
    }
}
