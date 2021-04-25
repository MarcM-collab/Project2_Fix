using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerApearence : MonoBehaviour
{
    public delegate void MoveUp();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SetRandomColor();
    }

    private void SetRandomColor()
    {
        Color col = new Color(Random.value, Random.value, Random.value, 1);
        GetComponent<Renderer>().material.color = col;
    }
}
