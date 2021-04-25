using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CardUsage : MonoBehaviour
{
    public float scale = 1f;
    private bool isDragging = false;
    private GameObject _gameObject;
    private GameObject _gameObjectCard;
    private Card _card;

    public HandManager HandManager;
    public Whiskas Whiskas;

    [SerializeField]
    public Tilemap _floorTilemap;
    [SerializeField]
    private Tilemap _collisionTilemap;

    public Camera _camera;

    private Vector3 mousePos;
    private Vector3 _currentGridPos;

    private void Update()
    {
        bool thereIsntAtile = !_collisionTilemap.HasTile(_floorTilemap.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition)));
        
        if (isDragging && InputManager.LeftMouseClick && thereIsntAtile && !IsEntity()) //si la carta está seleccionada y se pulsa en la escena, entra en la función.
        {
            if (_card is Unit)
            {
                spawn();
            }
            Whiskas.RemoveWhiskas(_card.Whiskas);
        }
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
    public void Draggin(GameObject gameObjectToSpawn, GameObject gameObjectCard)
    {
        //el primer gameobject es el "sprite" (objeto) que spwnea el carta (su estado morfologico),
        //el otro gameobject es la carta (referenciada para que pueda ser destruida al usarla)
        
        _gameObjectCard = gameObjectCard;
        _card = _gameObjectCard.GetComponent<Card>();

        if (EnoughWhiskas(_card))
        {
            _gameObject = gameObjectToSpawn;
            isDragging = true;//para saber si esta clicando(mas adelante posiblemente arrastre)
        }
    }
    private bool EnoughWhiskas(Card _card)//comprueba si hay whiskas (maná) suficiente para lanzar la carta
    {
        return _card.Whiskas <= Whiskas.currentWhiskas;
    }
    private void spawn()
    {
        Vector3 sizeTile = new Vector3(_floorTilemap.cellSize.x / 2, _floorTilemap.cellSize.y / 2, 0);
        mousePos = Input.mousePosition;

        _currentGridPos = _floorTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos)) + sizeTile;

        Instantiate(_gameObject, _currentGridPos, Quaternion.identity);
        HandManager.RemoveCard(_gameObjectCard.GetComponent<Card>());

        Destroy(_gameObjectCard);
        isDragging = false;
    }
}
