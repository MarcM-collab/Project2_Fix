using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    public static bool LeftMouseClick;

    private void Update()
    {
        LeftMouseClick = KeyPressed(KeyCode.Mouse0);
    }
    private bool KeyPressed(KeyCode key)
    {
        if (Input.GetKeyDown(key)) 
            return true;
        else 
            return false;
    }
}
