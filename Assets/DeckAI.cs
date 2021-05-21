using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardType
{
    public Card card;
    public int priority;
}

public class DeckAI : MonoBehaviour
{
    public List<CardType> IADeck;
}
