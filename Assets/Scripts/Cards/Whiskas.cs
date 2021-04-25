using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whiskas : MonoBehaviour
{
    public int currentWhiskas;
    public int maxWhiskas = 2;

    private Slider _slider;
    public int rounds = 0;
    public Text WhiskasText;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        currentWhiskas = 2;
        WhiskasText.text = currentWhiskas.ToString();
    }
    public void RestartWhiskas() //pasar turno (cada 2 turnos se hace el restart) 2 turnos: 1 player +  1 IA. => 1 Ronda.
    {
        maxWhiskas++;
        currentWhiskas = maxWhiskas;
        print(currentWhiskas);
        WhiskasText.text = maxWhiskas.ToString();
        _slider.value = (float)currentWhiskas / maxWhiskas; //por cambiar
        
    }
    public void RemoveWhiskas(int remove)
    {
       
        currentWhiskas -= remove;
        WhiskasText.text = currentWhiskas.ToString();
        _slider.value = (float)currentWhiskas / maxWhiskas;
        
    }
}
