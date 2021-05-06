using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Card
{
    public int Priority = 2;
    private Camera mainCamera;
    public virtual void ExecuteSpell() {}
    public virtual void IAUse() {}
    public virtual bool CanBeUsed() { return false; }
    protected Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    } 

    private void Start()
    {
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
    }
}
