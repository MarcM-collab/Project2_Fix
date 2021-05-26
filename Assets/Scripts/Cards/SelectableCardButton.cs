using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCardButton : MonoBehaviour
{
    public delegate void DisplayCard(Image gameObject, Card card);
    public static DisplayCard displayCard;
    private Image image;
    public Card card;
    private void Start()
    {
        image = GetComponent<Image>();
        
    }
    public void CardChosen()
    {
        gameObject.SetActive(false);

        displayCard?.Invoke(image, card);
        
    }

    
}
