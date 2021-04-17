using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public List<Card> Cards; //Baraja elegida por el player (8 cartas)

    List<Card> twoCardsRandom = new List<Card>();
    public Animation AnimatorCard;

    public Image[] buttons;

    public HandManager Hand;

    private void Start()
    {
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //de testeo
        {
            ShowRandomCards(ChooseRandom());
        }
    }

    private List<Card> ChooseRandom() //salen dos cartas randm y las guarda en una lista.
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


    public void ConfirmAddCard(int number)
    {
        Hand.AddCard(twoCardsRandom[number]);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        
        twoCardsRandom.Clear();

        //animaciones 
    }
}
