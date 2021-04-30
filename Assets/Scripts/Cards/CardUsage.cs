using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CardUsage : MonoBehaviour
{
    public float scale = 1f;
    public static bool isDragging = false;
    private GameObject _gameObject;
    private GameObject _gameObjectCard;
    private Card _card;

    public HandManager HandManager;

    [SerializeField]
    private Art _art;

    private Tilemap _floorTilemap => _art.FloorTilemap;
    private Tilemap _collisionTilemap => _art.CollisionTilemap;
    private Tilemap _uITilemap => _art.UITilemap;
    private Tile _allyTile => _art.AllyTile;

    private Camera _camera => _art.Camera;

    private Vector3 mousePos;
    private Vector3 _currentGridPos;

    public CardSpawner spawner;
    private Camera mainCamera;
    private Vector2 GetMouseTilePos
    {
        get 
        {
            mousePos = Input.mousePosition;
            var vector = _floorTilemap.WorldToCell(mainCamera.ScreenToWorldPoint(mousePos));
            return new Vector2(vector.x, vector.y);
        }
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        bool thereIsntAtile = !_collisionTilemap.HasTile(_floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition)));
        bool isSpawnableTile = _uITilemap.HasTile(_floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition)));

    }
    private bool IsEntity()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        var hitCollider = hit.collider;
        if (hitCollider != null)
        {
            var gameObject = hitCollider.gameObject;
            if (!(gameObject.GetComponent("Entity") as Entity is null))
            {
                return true;
            }
        }
        return false;
    }
    private void OnEnable()
    {
        ScriptButton._buttonCard += Draggin;
    }
    private void OnDisable()
    {
        ScriptButton._buttonCard -= Draggin;
    }
    public void Draggin(Unit card)
    {
        //el primer gameobject es el "sprite" (objeto) que spwnea el carta (su estado morfologico),
        //el otro gameobject es la carta (referenciada para que pueda ser destruida al usarla)

        _gameObjectCard = card.gameObject;
        _card = card;

        if (EnoughWhiskas(_card))
        {
            _gameObject = card.character.gameObject;
            isDragging = true;//para saber si esta clicando(mas adelante posiblemente arrastre)
        }
        else
        {
            StartCoroutine(NotEnough());
        }
    }

    private IEnumerator NotEnough()
    {
        _card.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _card.GetComponent<Image>().color = Color.white;
    }

    private bool EnoughWhiskas(Card _card)//comprueba si hay whiskas (man�) suficiente para lanzar la carta
    {
        return _card.Whiskas <= TurnManager.currentMana;
    }
    public void Spawn()
    {
        if (_card is Unit)
        {
            Vector2 pos = GetMouseTilePos;
            if (spawner.CheckPos(pos))
            {
                spawner.SpawnCard(_card, GetMouseTilePos, Team.TeamPlayer);
                isDragging = false;
                DestroyCard();
            }
        }
        //Vector3 sizeTile = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2, 0);
        //mousePos = Input.mousePosition;

        //_currentGridPos = _floorTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos)) + sizeTile;

        //Instantiate(_gameObject, _currentGridPos, Quaternion.identity);
        //HandManager.RemoveCard(_gameObjectCard.GetComponent<Card>());

        //Destroy(_gameObjectCard);
        //isDragging = false;
    }
    private void DestroyCard()
    {
        TurnManager.SubstractMana(_card.Whiskas);
        HandManager.RemoveCard(_card);
        Destroy(_card.gameObject);
    }
}
