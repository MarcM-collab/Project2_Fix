using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public CanvasGroup abilityDiplay;
    public Image spriteShower;
    public Image buttonUse;
    public Image whiskasIm;
    private Color buttonUseColor;
    private Color whiskasColor;
    public TMP_Text attackText;
    public TMP_Text abilityInfo;
    public TMP_Text abilityCost;

    private Camera mainCamera;
    private Character targetChar;
    private UseAbility currentAbility;
    private int currentAttack = 0;
    private Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    }

    private void Start()
    {
        spriteShower.color = new Color(spriteShower.color.r, spriteShower.color.g, spriteShower.color.b, 0);
        buttonUseColor = buttonUse.color;
        whiskasColor = whiskasIm.color;
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
        HideAbilityInfo();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayCast = Physics2D.Raycast(GetMousePosition, Vector2.zero);

            if (rayCast)
            {
                if (rayCast.transform.CompareTag("Character"))
                {
                    if (spriteShower.color.a <= 0)
                    {
                        spriteShower.color = new Color(spriteShower.color.r, spriteShower.color.g, spriteShower.color.b, 1);
                    }
                    targetChar = rayCast.transform.gameObject.GetComponent<Character>();
                    currentAttack = targetChar.AttackPoints;
                    ShowBasicInfo();
                    if (targetChar.Team == Team.TeamPlayer)
                    {
                        if (!targetChar.Exhausted)
                            ShowAbility();
                    }
                }
            }
        }
        if (targetChar)
        {
            if (currentAttack != targetChar.AttackPoints)
            {
                currentAttack = targetChar.AttackPoints;
                ShowBasicInfo();
            }
        }
    }
    private void ShowBasicInfo()
    {
        spriteShower.sprite = targetChar.GetComponent<SpriteRenderer>().sprite;
        attackText.text = targetChar.AttackPoints.ToString();

        HideAbilityInfo();
    }

    private void HideAbilityInfo()
    {
        abilityInfo.text = "";
        abilityDiplay.alpha = 0;
        abilityDiplay.interactable = false;
        abilityDiplay.blocksRaycasts = false;
        currentAbility = null; //resets and avoids casting when another char selected.
    }

    private void ShowAbility()
    {
        currentAbility = targetChar.GetComponentInChildren<UseAbility>();

        if (currentAbility)
        {
            abilityDiplay.alpha = 1;
            abilityDiplay.interactable = true;
            abilityDiplay.blocksRaycasts = true;
            abilityInfo.text = currentAbility.ability.textExplain;
            abilityCost.text = currentAbility.ability.whiskasCost.ToString();
        }
    }
    public void UseAbility()
    {
        if (currentAbility)
        {
            currentAbility.Use();
        }
        if (TurnManager.currentMana < currentAbility.ability.whiskasCost)
        {
            StartCoroutine(NotEnough(buttonUse, buttonUseColor));
            StartCoroutine(NotEnough(whiskasIm, whiskasColor));
        }
    }
    private IEnumerator NotEnough(Image i, Color startColor)
    {
        i.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        i.color = startColor;
    }
}
