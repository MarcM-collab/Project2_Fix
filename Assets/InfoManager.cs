using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public CanvasGroup abilityDiplay;
    public Image spriteShower;
    public Text attackText;
    public Text abilityInfo;
    public Text abilityCost;

    private Camera mainCamera;
    private Character targetChar;
    private UseAbility currentAbility;
    private Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    }

    private void Start()
    {
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
    }

    private void Update()
    {
        RaycastHit2D rayCast = Physics2D.Raycast(GetMousePosition, Vector2.zero);

        if(rayCast)
        {
            if (rayCast.transform.CompareTag("Character"))
            {
                targetChar = rayCast.transform.gameObject.GetComponent<Character>();
                ShowBasicInfo();
                if (targetChar.Team == Team.TeamPlayer)
                {
                    ShowAbility();
                }
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
        currentAbility = null; //resets and avoids casting when another char selected.
    }

    private void ShowAbility()
    {
        currentAbility = targetChar.GetComponentInChildren<UseAbility>();

        if (currentAbility)
        {
            abilityDiplay.alpha = 1;
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
    }
}
