using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Card
{
    [HideInInspector] public bool executed = false;
    public int Priority = 2;
    private Camera mainCamera;
    protected bool activated = false;
    protected TileManager tileManager;
    protected Vector3Int prevPos;
    private void OnEnable()
    {
        ScriptButton.spellButton += Dragging;
    }
    private void OnDisable()
    {
        ScriptButton.spellButton -= Dragging;
    }
    public virtual void ExecuteSpell() 
    {
        activated = false;
    }
    private void Dragging(Spell s) 
    {
        activated = true;
    }
    public virtual void IAUse() {}
    public virtual bool CanBeUsed() { return false; }
    protected Vector2 GetMousePosition
    {
        get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
    }
    protected Vector3Int GetIntPos(Vector3 pos)
    {
        return tileManager.FloorTilemap.WorldToCell(pos);
    }
    private void Awake()
    {
        mainCamera = Camera.main; //This will avoid extra iterations searching for a Game Object with tag in the whole scene.
        tileManager = FindObjectOfType<TileManager>();
    }
}
