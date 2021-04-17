using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public List<Card> Cards; //Baraja elegida por el player (8 cartas)

    public List<Card> Hand; //la mano actual del jugador

    List<Card> twoCardsRandom = new List<Card>();
    public Animation AnimatorCard;

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

        print(twoCardsRandom.Count);
        return twoCardsRandom;
    }

    private void ShowRandomCards(List<Card> _twoCardsRandom) //muestra las dos cartas random
    {
        Vector3 nextPos = new Vector3(0, 0.5f, 0);

        for (int i = 0; i < _twoCardsRandom.Count; i++)
        {
            Instantiate(_twoCardsRandom[i], nextPos, Quaternion.identity);
            nextPos.x += 0.5f;
        }
    }


    private void DiscardCard()
    {

        //si la q escoges es == a la de la lista 
        //guardas else destroy(laotra)

    }
}
