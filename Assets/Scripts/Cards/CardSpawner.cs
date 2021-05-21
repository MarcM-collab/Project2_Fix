using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap CollisionTileMap;
    public Transform characters;
    public Camera Camera;

    public Color IAColor;
    public Color PlayerColor;

    public Sprite healthEnemySprite;

    public void SpawnCard(Card toSpawn, Vector2 pos, Team team)
    {
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
                theTile.gameObject.GetComponentInChildren<SetHealthText>().GetComponent<Image>().sprite = healthEnemySprite;
                RectTransform rt = theTile.gameObject.GetComponentInChildren<TMP_Text>().rectTransform;
                rt.localScale = new Vector3(-rt.localScale.x, rt.localScale.y, rt.localScale.z);
                break;
        }
        colorSetter.SetValue(1);
    }
}
