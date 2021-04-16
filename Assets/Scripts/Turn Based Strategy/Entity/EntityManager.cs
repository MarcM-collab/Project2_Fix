using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityManager
{
    private static List<Entity> Entities;
    private static int _currentTargetIndex = 1;
    private static int _currentExecutorIndex = 0;

    public static Entity TargetEntity => Entities[_currentTargetIndex];
    public static Entity ExecutorEntity => Entities[_currentExecutorIndex];

    [RuntimeInitializeOnLoadMethod]
    private static void InitEntities()
    {
        Entities = Object.FindObjectsOfType<Entity>().ToList();
    }

    public static void SetExecutor(Entity executor)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            if (Entities[i] == executor)
            {
                _currentExecutorIndex = i;
                return;
            }
        }
    }
    public static void SetTarget(Entity target)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            if (Entities[i] == target)
            {
                _currentTargetIndex = i;
                return;
            }
        }
    }
    public static Entity[] GetEnemies(Entity entity)
    {
        return Entities.Where(x => x.Team != entity.Team).ToArray();
    }

    public static Entity[] GetLivingEnemies(Entity entity)
    {
        return Entities.Where(x => x.Team != entity.Team && x.IsAlive).ToArray();
    }
    public static Entity[] GetAllies(Entity entity)
    {
        return Entities.Where(x => x.Team == entity.Team).ToArray();
    }
    public static Entity[] GetLivingAllies(Entity entity)
    {
        return Entities.Where(x => x.Team == entity.Team && x.IsAlive).ToArray();
    }
    public static Entity[] GetActiveAllies(Entity entity)
    {
        return Entities.Where(x => x.Team == entity.Team && x.IsActive).ToArray();
    }
    public static Entity[] GetActiveAllies(Team team)
    {
        return Entities.Where(x => x.Team == team && x.IsActive).ToArray();
    }
    public static bool IsEntityInList(Entity[] list, Entity entitySearched)
    {
        foreach (Entity entity in list)
        {
            if (entity == entitySearched)
                return true;
        }
        return false;
    }
}
