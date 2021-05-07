using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDrawableCardsPlayer : MonoBehaviour
{
    [SerializeField]
    private List<Card> Cards; //Baraja elegida por el player (8 cartas)

    List<Card> twoCardsRandom = new List<Card>(); //lista para las dos cartas aleatorias.

    [SerializeField]
    private Image[] buttons;

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

    private void ChooseDrawableCardsEnter(Animator animator)
    {
        var IsBelowHandMaxSize = Hand.hand.Count < maxCardInHand;
        if (IsBelowHandMaxSize)
            ShowRandomCards(ChooseRandom());
        else
        {
            animator.SetBool("ChooseCard", false);
            cardSelected = false;
            TurnManager.CardDrawn = true;
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
    private void ShowRandomCards(List<Card> _twoCardsRandom) //muestra las dos cartas random
    {
        for (int i = 0; i < _twoCardsRandom.Count; i++)
        {
            var sprite = _twoCardsRandom[i].GetComponent<Image>().sprite;

            buttons[i].sprite = sprite;
            buttons[i].gameObject.SetActive(true);
        }

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
    public void ConfirmAddCard(int number) //se pasa información de la carta escogida y se desactivan después.
    {
        Hand.AddCard(twoCardsRandom[number]);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        twoCardsRandom.Clear();
        cardSelected = true;
    }
}
