using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDrawableCardsPlayer : MonoBehaviour
{
    private List<Card> _cards; //Baraja elegida por el player (8 cartas)
    [SerializeField]
    private DeckPlayer _deckPlayer;

    Card[] twoCardsRandom = new Card[2]; //lista para las dos cartas aleatorias.
    private List<Card> randomControl = new List<Card>();
    [SerializeField]
    private Image[] buttons;
    private RectTransform[] cardInstancePos = new RectTransform[2];
    private GameObject[] cardsGO = new GameObject[2];

    [SerializeField]
    private HandManager Hand;
    [SerializeField]
    private int maxCardInHand = 6;
    private bool currentTurn = false;

    private bool PressedFirst;

    private bool cardSelected;

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
    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            cardInstancePos[i] = buttons[i].GetComponentInChildren<RectTransform>();
        }
    }
    private void Start()
    {
        _cards = _deckPlayer.Cards;
    }
    private void ChooseDrawableCardsEnter(Animator animator)
    {
        if (Hand.hand.Count < maxCardInHand)
        {
            RemovePreviousCards();
            ChooseRandom();
            ShowRandomCards();
        }

        else
        {
            animator.SetBool("ChooseCard", false);
            cardSelected = false;
            TurnManager.CardDrawn = true;
        }
    }

    private void RemovePreviousCards()
    {
        for (int i = 0; i < twoCardsRandom.Length; i++)
        {
            Destroy(cardsGO[i]);
            cardsGO[i] = null;
        }
    }

    private void ChooseDrawableCardsUpdate(Animator animator)
    {
        if (cardSelected)
        {
            animator.SetBool("ChooseCard", false);
            cardSelected = false;
            TurnManager.CardDrawn = true;
        }
    }
    private void ShowRandomCards() //muestra las dos cartas random
    {
        for (int i = 0; i < twoCardsRandom.Length; i++) 
        {
            buttons[i].gameObject.SetActive(true);

            cardsGO[i] = Instantiate(twoCardsRandom[i].gameObject, buttons[i].transform);

            cardsGO[i].GetComponent<ScriptButton>().enabled = false;
            cardsGO[i].GetComponent<Button>().enabled = false;

            CursorUIShower ui = cardsGO[i].GetComponent<CursorUIShower>();
            if (ui)
                ui.use = true;

            RectTransform rt = cardsGO[i].GetComponent<RectTransform>();
            rt.position = cardInstancePos[i].position;
            rt.localScale = cardInstancePos[i].localScale; 
        }

    }
    private void ChooseRandom() //salen dos cartas random y las guarda en una lista.
    {
        int random1 = UnityEngine.Random.Range(0, _cards.Count);
        int random2 = UnityEngine.Random.Range(0, _cards.Count);

        while (random1 == random2)
        {
            random2 = UnityEngine.Random.Range(0, _cards.Count);
        }
        twoCardsRandom[0] = _cards[random1];
        twoCardsRandom[1] = _cards[random2];
    }
    public void ConfirmAddCard(int number) //se pasa información de la carta escogida y se desactivan después.
    {
        Hand.AddCard(twoCardsRandom[number]);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        cardSelected = true;
    }
}
