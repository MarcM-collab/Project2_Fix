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
    public Whiskas Whiskas;

    private void Start()
    {
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
    }
    public void PassTurn()
    {
        Whiskas.rounds++;
        if (Whiskas.rounds % 2 == 0)
        {
            Whiskas.RestartWhiskas();
        }
         if(Hand.hand.Count < maxCardInHand)//límite de cartas en mano.
            ShowRandomCards(ChooseRandom());    
    }
    private List<Card> ChooseRandom() //salen dos cartas random y las guarda en una lista.
    {
        int random1 = Random.Range(0, Cards.Count);
        int random2 = Random.Range(0, Cards.Count);

        while (random1 == random2)
        {
            random2 = Random.Range(0, Cards.Count);
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

    public void ConfirmAddCard(int number) //se pasa información de la carta escogida y se desactivan después.
    {
        Hand.AddCard(twoCardsRandom[number]);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        twoCardsRandom.Clear();
    }
}
