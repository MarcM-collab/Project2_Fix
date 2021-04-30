using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public string Text1, Text2, IAText;
    public Color text1Color, text2Color, IAColor;
    private Text txt;
    private TurnManager turn;
    private Image im;
    private bool hasToChange = true;
    private void Start()
    {
        im = GetComponent<Image>();
        turn = FindObjectOfType<TurnManager>();
        txt = GetComponentInChildren<Text>();
        txt.text = Text2;
    }
    public void OnClick()
    {
        if (txt.text == Text1)
        {
            im.color = text2Color;
            txt.text = Text2;
            hasToChange = false;
        }
        else
        {
            im.color = IAColor;
            txt.text = IAText;
            hasToChange = true;
        }
    }
    private void Update()
    {
        if (txt.text != Text1 && turn.PlayerTurn && hasToChange)
        {
            txt.text = Text1;
            im.color = text1Color;
        }
    }
}
