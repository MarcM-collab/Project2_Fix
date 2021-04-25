using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private List<Entity> Entities;
    private int _currentIndex;

    public Entity ActiveEntity => Entities[_currentIndex];

    
   
    public void SetNextEntity()
    {
        _currentIndex++;
        _currentIndex = _currentIndex % Entities.Count;
    }
}
