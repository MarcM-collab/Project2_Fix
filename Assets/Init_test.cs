using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        EntityManager.InitEntities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
