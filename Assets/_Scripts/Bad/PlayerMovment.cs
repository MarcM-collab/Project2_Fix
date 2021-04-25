using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    public float Speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            MoveLeft();
        if (Input.GetKey(KeyCode.D))
            MoveRight();
        if (Input.GetKey(KeyCode.W))
            MoveUp();
        if (Input.GetKey(KeyCode.S))
            MoveDown();

        if (Input.GetKeyDown(KeyCode.W))
            Debug.Log("W pressed (just once)");
        if (Input.GetKey(KeyCode.W))
            Debug.Log("W pressed (while being pressed)");
        if (Input.GetKeyUp(KeyCode.W))
            Debug.Log("W released");


        Debug.Log("Horizontal axis: " + Input.GetAxis("Horizontal"));
    }

    private void MoveDown()
    {
        transform.Translate(Vector2.down * Speed * Time.deltaTime);
    }

    private void MoveUp()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void MoveRight()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    private void MoveLeft()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
    }
}
