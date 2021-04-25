using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterManager
{
    private static List<Character> Characters;
    private static int _currentTargetIndex = 1;
    private static int _currentExecutorIndex = 0;

    public static Character TargetCharacter => Characters[_currentTargetIndex];
    public static Character ExecutorCharacter => Characters[_currentExecutorIndex];

    [RuntimeInitializeOnLoadMethod]
    private static void InitEntities()
    {
        Characters = Object.FindObjectsOfType<Character>().ToList();
    }

    public static void SetExecutor(Character executor)
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Characters[i] == executor)
            {
                _currentExecutorIndex = i;
                return;
            }
        }
    }
    public static void SetTarget(Character target)
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Characters[i] == target)
            {
                _currentTargetIndex = i;
                return;
            }
        }
    }
    public static Character[] GetCharacters(Team team)
    {
        return Characters.Where(x => x.Team != team).ToArray();
    }

    public static Character[] GetLivingCharacters(Team team)
    {
        return Characters.Where(x => x.Team != team && x.IsAlive).ToArray();
    }
    public static Character[] GetActiveCharacters(Team team)
    {
        return Characters.Where(x => x.Team == team && x.IsActive).ToArray();
    }
    public static bool IsEntityInList(Character[] list, Character entitySearched)
    {
        foreach (Character entity in list)
        {
            if (entity == entitySearched)
                return true;
        }
        return false;
    }
    public static void RemoveExhaust()
    {
        foreach (Character entity in Characters)
        {
            entity.Exhausted = false;
        }
    }
}
