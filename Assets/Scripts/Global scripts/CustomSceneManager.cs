using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public bool testing = false;
    private void Awake()
    {
        if (testing)
            OnLevelWasLoaded(1);
        else
            DontDestroyOnLoad(transform.gameObject);
    }
    public static void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level != 0) //is not the menu
        {
            //TurnManager.NextTurn();
            //TurnManager.Starting();
            //EntityManager.InitEntities();
        }
    }

}
