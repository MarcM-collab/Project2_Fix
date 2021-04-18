using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUsage : MonoBehaviour
{
    
    
    public float scale = 1f;
    int oneTouch = 0;
    public void Draggin(GameObject _sprite)
    {
        
          print("e");
            Instantiate(_sprite.gameObject, new Vector3(0,0,0), Quaternion.identity);
            oneTouch++;
        
        



    }
}
