using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    [Range(0f, 2f)] public float timeToDisplay = 0.25f;
    private float currentTime = 0;
    public MenuPanel buttonShow;
    public Abilty ability;
    public int whiskasCost = 1;
    private void OnMouseOver()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeToDisplay)
        {
                buttonShow.Show();
        }
    }
    private void OnMouseExit()
    {
        currentTime = 0;
        buttonShow.Hide();

    }

    public void Use()
    {
        print("Health!!");
        ability.Excecute();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            HealthSystem.TakeDamage(2, CharacterManager.ExecutorCharacter);
            print("Damaged!");
        }
    }
}
