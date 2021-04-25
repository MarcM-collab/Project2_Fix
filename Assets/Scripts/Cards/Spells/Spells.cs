using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : Card
{
    public int Priority = 2;
    private Camera mainCamera;
    public virtual void ExecuteSpell() {}
    protected Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    } 

    private void Start()
    {
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
    }
}
