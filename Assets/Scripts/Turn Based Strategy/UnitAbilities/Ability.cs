using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilty : MonoBehaviour
{
    [HideInInspector] public bool executed = false;
    private Camera mainCamera;

    protected Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    }

    private void Start()
    {
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
    }

    public int whiskasCost;
    public string textExplain;
    public virtual void Excecute() { }
    public virtual void IAExecute() { }
}
