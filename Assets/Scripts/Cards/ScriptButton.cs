using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptButton : MonoBehaviour
{
    //script para que los botones pasaran correctamente así mismo y prefabs.
    public delegate void ButtonCard(GameObject gameObject, GameObject cardName); 
    public static ButtonCard _buttonCard;
    public void ClicButton()
    {
        _buttonCard?.Invoke(GetComponent<Unit>().sprite, gameObject); //el primero es la sprite de la carta (la morphologia) y el segundo es el boton que contiene esta script.
    }    
}
