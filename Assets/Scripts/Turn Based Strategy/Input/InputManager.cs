using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    public static bool LeftMouseClick;
    public GameObject gOAI;
    public GameObject gOPlayer;

    private void Update()
    {
        LeftMouseClick = KeyPressed(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (EntityManager.TeamPlaying == Team.TeamAI)
            {
                gOAI.SetActive(false);
                gOPlayer.SetActive(true);
            }
            else
            {
                gOAI.SetActive(true);
                gOPlayer.SetActive(false);
            }
            EntityManager.SwapTeams();
        }
    }
    private bool KeyPressed(KeyCode key)
    {
        if (Input.GetKeyDown(key)) 
            return true;
        else 
            return false;
    }
}
