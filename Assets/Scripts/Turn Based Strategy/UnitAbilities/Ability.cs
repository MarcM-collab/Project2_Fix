using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilty : MonoBehaviour
{
    [HideInInspector] public bool executed = false;
    private Camera mainCamera;

    protected Vector2 GetMousePosition
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    public int whiskasCost;
    public string textExplain;
    public virtual void Excecute() { }
    public virtual void IAExecute() { }
}
