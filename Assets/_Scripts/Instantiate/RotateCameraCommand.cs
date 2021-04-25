using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraCommand : ICommand
{
    public void Excecute()
    {
        Camera.main.transform.Rotate(new Vector3(0, 0, 45));
    }

    public void Undo()
    {
        Camera.main.transform.Rotate(new Vector3(0, 0, -45));
    }

    
}
