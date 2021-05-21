using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Riccochet : Abilty
{
    //public int damage = 2;
    public int numOfTargets = 2;
    public float waitForNextTarget;
    private List<Character> damagedCharacters = new List<Character>();
    private Character selfChar;
    private void Start()
    {
        selfChar = GetComponent<Character>();
    }
    public override void Excecute()
    {
        Use(Team.TeamAI);
    }
    public override void IAExecute()
    {
        Use(Team.TeamPlayer);
    }
    private void Use(Team targetTeam)
    {
        Character[] targets = EntityManager.GetCharacters(targetTeam);
        Character target = targets[UnityEngine.Random.Range(0, targets.Length)];

        Damage(target, selfChar);

        StartCoroutine(FindNewTargets(targetTeam, target));
        //RaycastHit2D hit2D = Physics2D.Raycast(GetMousePosition, Vector2.zero);

        //if (hit2D)
        //{
        //    if (hit2D.transform.CompareTag("Character"))
        //    {
        //        Character target = hit2D.collider.gameObject.GetComponent<Character>();
        //        if (target.Team == targetTeam)
        //        {
        //            HealthSystem.TakeDamage(damage, target);
        //            FindNewTargets(targetTeam, target);
        //        }
        //    }
        //}
    }
    private IEnumerator FindNewTargets(Team team, Character currentTarget)
    {
        for (int i = 0; i < numOfTargets; i++)
        {
            yield return new WaitForSeconds(waitForNextTarget);
            damagedCharacters.Add(currentTarget);
            currentTarget = GetNearTarget(currentTarget, team);

            if (!currentTarget)
                break;

            Damage(currentTarget, selfChar);
        }
        executed = true;
    }

    private Character GetNearTarget(Character currentTarget, Team team)
    {
        List<Character> inGameCharacters = EntityManager.GetCharacters(team).ToList();

        foreach (var item in damagedCharacters)
        {
            if (inGameCharacters.Contains(item))
            {
                inGameCharacters.Remove(item);
            }
        }
        if (inGameCharacters.Count <= 0)
            return null;
        
        Character nearestChar = inGameCharacters[0];

        for (int i = 1; i < inGameCharacters.Count; i++)
        {
            if (CheckDistance(nearestChar.transform.position, inGameCharacters[i].transform.position, currentTarget.transform.position))
            {
                nearestChar = inGameCharacters[i];
            }
        }
        return nearestChar;
    }
    private bool CheckDistance(Vector3 nearest, Vector3 toTest, Vector3 current)
    {
        return Vector2.Distance(nearest, current) > Vector2.Distance(toTest, current);
    }
    private void Damage(Character target, Character executor)
    {
        EntityManager.SetExecutor(executor);
        EntityManager.SetTarget(target);

        target.Hit = true;
    }
}
