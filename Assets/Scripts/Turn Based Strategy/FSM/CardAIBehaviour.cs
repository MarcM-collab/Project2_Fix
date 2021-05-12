using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CardAIBehaviour : MonoBehaviour
{
    protected static List<Card> IAHand;
    private void Start()
    {
        IAHand = new List<Card>();
    }
}
