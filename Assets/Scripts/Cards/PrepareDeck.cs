using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class PrepareDeck : MonoBehaviour
{
    public Image[] Slots;
    public ChooseDrawableCardsPlayer CardDeck;
    private List<Card> currentCards = new List<Card>();
    private int index= 0;
    private int DeckLimitation = 8;

    [SerializeField]
    private List<SelectableCardButton> cardDisplays;

    public GameObject CombatBehaviour;

    private CanvasGroup canvas;
    private void Start()
    {
        for (int i = index; i < Slots.Length; i++)
        {
            Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 0);
        }

        canvas = GetComponent<CanvasGroup>();
        cardDisplays = FindObjectsOfType<SelectableCardButton>().ToList();
    }

    private void OnDisable()
    {
        SelectableCardButton.displayCard -= CardDisplayChoosen;
    }

    private void OnEnable()
    {
        SelectableCardButton.displayCard += CardDisplayChoosen;

    }

    public void CardDisplayChoosen(Image cardDisplay, Card card)
    {
  
        if(index < Slots.Length)
        {
         
            Slots[index].sprite = cardDisplay.sprite;
            Slots[index].color = new Color(Slots[index].color.r, Slots[index].color.g, Slots[index].color.b, 1);
            currentCards.Add(card);
            if (index >= Slots.Length)
                index = 0;
   
            index++;
            for (int i = 0; i < currentCards.Count; i++)
            {
                print(currentCards[i].name);
            }
        }
    }

    public void RemoveChosenCard(int _index)
    {
        for (int i = _index; i < Slots.Length-1; i++)
        {
            if(Slots[i + 1].sprite != null)
                Slots[i].sprite = Slots[i + 1].sprite;
            
        }
        for (int i = 0; i < cardDisplays.Count; i++)
        {
            if (cardDisplays[i].name == currentCards[_index].name)
                cardDisplays[i].gameObject.SetActive(true);
        }

        Slots[Slots.Length-1].color = new Color(Slots[_index].color.r, Slots[_index].color.g, Slots[_index].color.b, 0);
        currentCards.RemoveAt(_index);
        index--; 
    }

    public void DeckIsFinished()
    {
        if (currentCards.Count == DeckLimitation)
        {
            for (int i = 0; i < currentCards.Count; i++)
            {
                CardDeck.Cards[i] = currentCards[i];
            }

            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;

            CombatBehaviour.gameObject.SetActive(true);
            TurnManager.NextTurn();
        }
    }
}
