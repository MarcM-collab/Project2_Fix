using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap CollisionTileMap;
    private List<Vector2> testedPos = new List<Vector2>();
    public Transform characters;

    public Color IAColor;
    public Color PlayerColor;

    public void SpawnCard(Card toSpawn, Vector2 pos, Team team)
    {
        if (!CheckPos(pos))
            return;

        GameObject theTile = Instantiate(toSpawn.GetComponent<Unit>().character.gameObject, pos, Quaternion.identity);
        theTile.transform.position = new Vector3(pos.x + (FloorTileMap.cellSize.x / 2), pos.y + (FloorTileMap.cellSize.y / 2), 0);

        theTile.GetComponent<Character>().Team = team;

        if (team == Team.TeamPlayer)
        {
            theTile.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            theTile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        theTile.transform.SetParent(characters);
        EntityManager.ActualizeEntities();

        HealthBar colorSetter = theTile.gameObject.GetComponentInChildren<HealthBar>();
        switch (team)
        {
            case Team.TeamPlayer:
                colorSetter.color = PlayerColor;
                break;
            case Team.TeamAI:
                colorSetter.color = IAColor;
                break;
        }
        colorSetter.SetValue(1);
    }
    public Vector2 GetValidRandomPos(int minX, int maxX, int minY, int maxY)
    {
        int tilesLength = 0;
        for (int i = minX; i < maxX; i++)
        {
            for (int v = minY; v < maxY; v++)
            {
                tilesLength++;
            }
        }
        testedPos.Clear();
        Vector2 pos;
        do
        {
            pos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));

            if (!testedPos.Contains(pos))
                testedPos.Add(pos);

            if (testedPos.Count >= tilesLength)
            {
                print("ALL TESTED");
                break;
            }
        }
        while (!CheckPos(pos));

        return pos;
    }
    public bool CheckPos(Vector2 pos)
    {
        RaycastHit2D rayCast = Physics2D.Raycast(pos, Vector2.zero);

        if (rayCast)
        {
            if (rayCast.transform.CompareTag("Character"))
            {
                return false;
            }
        }
        return true;
    }
}
