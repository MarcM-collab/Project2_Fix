using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<Card> hand;
    [SerializeField]
    private float scale = 1f;
    public Transform HandCanvas;

    public void AddCard(Card newCard)
    {
        Transform cardInstance = Instantiate(newCard, HandCanvas.position, Quaternion.identity).transform;
        cardInstance.name = cardInstance.name + hand.Count;
        
        hand.Add(cardInstance.GetComponent<Card>());
        
        cardInstance.SetParent(HandCanvas);
        cardInstance.localScale = new Vector3(scale,scale,scale);//escalamos las cartas que se ven en la mano.
    }

    public void RemoveCard(Card cardToRemove)
    {
        for (int i = 0; i < hand.Count; i++) //en el if hay que buscar una solución menos compleja.
        {
            if(hand[i].name.Substring(hand[i].name.Length - 1 - hand.Count / 10) == cardToRemove.name.Substring(cardToRemove.name.Length - 1-hand.Count/10)) //el .length con más de 10 cartas en la mano no funciona.
            {
                hand.Remove(hand[i]);
                break;
            }
        }
        OrderHand();
    }

    private void OrderHand() //remueve los numeros que se han colocado anteriomente, de nuevo coloca nuevos numeros ordenados.
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].name = hand[i].name.Remove(hand[i].name.Length - 1 - hand.Count / 10);
            hand[i].name = hand[i].name + i;
        }
    }
}
