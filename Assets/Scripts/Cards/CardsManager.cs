using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public List<Card> Cards; //Baraja elegida por el player (8 cartas)

    List<Card> twoCardsRandom = new List<Card>(); //lista para las dos cartas aleatorias.

    public Image[] buttons;

    public HandManager Hand;
    [SerializeField]
    private int maxCardInHand = 6;
    private bool currentTurn = false;

    private TurnManager turn;
    private void Start()
    {
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        turn = FindObjectOfType<TurnManager>();
    }
    private void Update()
    {
        if (turn.PlayerTurn && !currentTurn)
        {
            currentTurn = true; //avoids extra iterations

            if (Hand.hand.Count < maxCardInHand) //l�mite de cartas en mano. 
                ShowRandomCards(ChooseRandom());
        }
    }
    public void PassTurn()
    {
        if (turn.PlayerTurn)
            NextTurn(); //can't click the button if it's not the player turn
    }

    private void NextTurn()
    {
        turn.NextTurn();
        currentTurn = false;
    }

    private List<Card> ChooseRandom() //salen dos cartas random y las guarda en una lista.
    {
        int random1 = UnityEngine.Random.Range(0, Cards.Count);
        int random2 = UnityEngine.Random.Range(0, Cards.Count);

        while (random1 == random2)
        {
            random2 = UnityEngine.Random.Range(0, Cards.Count);
        }
        twoCardsRandom.Add(Cards[random1]);
        twoCardsRandom.Add(Cards[random2]);

        return twoCardsRandom;
    }

    private void ShowRandomCards(List<Card> _twoCardsRandom) //muestra las dos cartas random
    {
        for (int i = 0; i < _twoCardsRandom.Count; i++)
        {
            buttons[i].sprite = _twoCardsRandom[i].GetComponent<Image>().sprite;
            buttons[i].gameObject.SetActive(true);
        }
    }

    public void ConfirmAddCard(int number) //se pasa informaci�n de la carta escogida y se desactivan despu�s.
    {
        Hand.AddCard(twoCardsRandom[number]);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        twoCardsRandom.Clear();
    }
}
