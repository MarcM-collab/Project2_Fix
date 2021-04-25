using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeOrbCommand : ICommand
{
    private Color _color;
    private GameObject _prefab;
    private Vector2 _position;

    public MakeOrbCommand(GameObject prefab, Vector2 position, Color color)
    {
        _color = color;
        _prefab = prefab;
        _position = position;

        
    }

    public void Excecute()
    {
        OrbSpawner.MakeOrb(_prefab, _position, _color);
    }

    public void Undo()
    {
        OrbSpawner.RemoveOrb(_prefab, _position, _color);
    }
}
