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

    private void Start()
    {
        _slider = GetComponent<Slider>();
        currentWhiskas = 2;
    }
    public void RestartWhiskas() //pasar turno (cada 2 turnos se hace el restart) 2 turnos: 1 player +  1 IA. => 1 Ronda.
    {
        maxWhiskas++;
        currentWhiskas = maxWhiskas;
        _slider.value = currentWhiskas / maxWhiskas;
    }
    public void RemoveWhiskas(int remove)
    {
       
        currentWhiskas -= remove;
        _slider.value = currentWhiskas / maxWhiskas;
    }
}
