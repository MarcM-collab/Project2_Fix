using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetExplanationSprite : MonoBehaviour
{
    private Image im;
    void Start()
    {
        im = GetComponent<Image>();
        SetSpriteTransparency(0);
    }
    private void OnEnable()
    {
        ScriptButton.spellButton += SetCardInfo;
        ScriptButton._buttonCard += SetCardInfo;
    }
    private void OnDisable()
    {
        ScriptButton.spellButton -= SetCardInfo;
        ScriptButton._buttonCard -= SetCardInfo;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && im.color.a > 0)
        {
            SetSpriteTransparency(0);
        }
    }
    private void SetSpriteTransparency(float transparency)
    {
        im.color = new Color(im.color.r, im.color.g, im.color.b, transparency);
    }
    private void SetCardInfo(Card c)
    {
        SetSpriteTransparency(1);
    }
}
