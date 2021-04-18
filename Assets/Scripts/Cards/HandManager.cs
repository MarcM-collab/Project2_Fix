using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<Card> hand;
    public float scale = 1f;
    public Transform HandCanvas;
    public void AddCard(Card newCard)
    {
        hand.Add(newCard);
        Transform cardInstance = Instantiate(newCard, HandCanvas.position, Quaternion.identity).transform;
        cardInstance.SetParent(HandCanvas);
        cardInstance.localScale = new Vector3(scale,scale,scale);
    }
}
