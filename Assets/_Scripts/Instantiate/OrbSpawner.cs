using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    static List<GameObject> Orbs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void MakeOrb(GameObject prefab, Vector2 position, Color color)
    {
        GameObject go = GameObject.Instantiate(prefab, position, Quaternion.identity);
        go.GetComponent<SpriteRenderer>().color = color;

        if (Orbs == null)
            Orbs = new List<GameObject>();

        Orbs.Add(go);
    }

    public static void RemoveOrb(GameObject prefab, Vector2 position, Color color)
    {
        for (int i = 0; i < Orbs.Count; i++)
        {
            if(AreEqual(Orbs[i], position, color))
            {
                Destroy(Orbs[i]);
                Orbs.RemoveAt(i);
                return;
            }
                
        }
    }

    private static bool AreEqual(GameObject gameObject, Vector2 position, Color color)
    {
        return (Vector2)gameObject.transform.position == position &&
            gameObject.GetComponent<SpriteRenderer>().color == color;
    }
}
