using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosHandZoomButton : MonoBehaviour
{
    public GameObject show;
    public PosHand posHand;
    private void OnEnable()
    {
        ScriptButton.spellButton += SetDisabledCard;
        ScriptButton._buttonCard += SetDisabledCard;
    }
    private void OnDisable()
    {
        ScriptButton.spellButton -= SetDisabledCard;
        ScriptButton._buttonCard -= SetDisabledCard;
    }
    public void SetActivated()
    {
        show.SetActive(true);
    }
    public void SetDisabled()
    {
        show.SetActive(false);
    }
    public void SetDisabledCard(Card c)
    {
        SetActivated();
        posHand.Hide();
    }
}
