using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    private GameObject AI;
    [SerializeField]
    private GameObject Player;

    private void OnEnable()
    {
        TurnManager.OnSwitchBehaviour += Switch;
    }
    private void OnDisable()
    {
        TurnManager.OnSwitchBehaviour -= Switch;
    }
    private void Switch()
    {
        if (AI.activeSelf)
        {
            AI.SetActive(false);
            Player.SetActive(true);
        }
        else
        {
            AI.SetActive(true);
            Player.SetActive(false);
        }
    }
}
