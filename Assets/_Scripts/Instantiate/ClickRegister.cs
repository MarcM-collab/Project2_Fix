using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClickRegister : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject MoonPrefab;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MakeOrb(StarPrefab);
        if (Input.GetMouseButtonDown(1))
            MakeOrb(MoonPrefab);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Invoker.ExecuteAll();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateCamera();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Invoker.Undo();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Invoker.Redo();
        }
    }

    

    public static void MakeOrb(GameObject prefab)
    {
        var clickPosition = GetClickPosition();
        var color = GetRandomColor();
      
        if (clickPosition!=null)
        {
            var command = new MakeOrbCommand(prefab, (Vector2)clickPosition,color);
            Invoker.AddCommand(command);

        }
        
    }

    private void RotateCamera()
    {
        var command = new RotateCameraCommand();
        Invoker.AddCommand(command);
    }



    private static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1);
    }

    private static Vector2? GetClickPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.point;
        }
        return null;
    }
}
