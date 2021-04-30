using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityManager
{
    public static Team TeamPlaying = Team.TeamAI;
    private static List<Entity> Entities;
    private static int _currentTargetIndex = 1;
    private static int _currentExecutorIndex = 0;

    public static Entity TargetCharacter => Entities[_currentTargetIndex];
    public static Character ExecutorCharacter => Entities[_currentExecutorIndex] as Character;

    public static bool TurnChaged;

    [RuntimeInitializeOnLoadMethod]
    private static void InitEntities()
    {
        Entities = Object.FindObjectsOfType<Entity>().ToList();
    }
    public static void ActualizeEntities()
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
    public static Character[] GetCharacters(Team team)
    {
        return CharactersInList(Entities.Where(x => x.Team == team).ToArray());
    }

    public static Character[] GetLivingCharacters(Team team)
    {
        return CharactersInList(Entities.Where(x => x.Team == team && x.IsAlive).ToArray());
    }
    public static Character[] GetActiveCharacters(Team team)
    {
        return CharactersInList(Entities.Where(x => x.Team == team && x.IsActive).ToArray());
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
    public static void RemoveExhaust()
    {
        if (Entities == null) //Avoids null reference
            InitEntities();

        foreach (Entity character in Entities)
        {
            character.Exhausted = false;
        }
    }
    private static Character[] CharactersInList(Entity[] entities)
    {
        List<Character> characters = new List<Character>();
        for (int i = 0; i < entities.Length; i++)
        {
            if (!(entities[i].GetComponent("Character") as Character is null))
            {
                characters.Add(entities[i] as Character);
            }
        }
        return characters.ToArray();
    }
}
