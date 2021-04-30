using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosHand : MonoBehaviour
{
    public float yDoZoom = 244.6f, yStopZoom = 442.2f;
    public float xStopZoomLeft = 400f, xStopZoomRight = 1400f;
    public Vector3 scale;
    private Vector3 startScale;
    private RectTransform rectTransform;
    public float yOffset = -302.8f;
    private float yStart;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startScale = rectTransform.localScale;
        yStart = rectTransform.position.y;
    }
    private void Update()
    {
        if (Input.mousePosition.y < yDoZoom && (Input.mousePosition.x < xStopZoomRight && Input.mousePosition.x > xStopZoomLeft))
        {
            if (TurnManager.TeamTurn == Team.TeamPlayer)
                Show();
        }
        else if ((Input.mousePosition.y > yStopZoom || (Input.mousePosition.x > xStopZoomRight || Input.mousePosition.x < xStopZoomLeft)) || TurnManager.TeamTurn == Team.TeamPlayer)
        {
            Hide();
        }
    }
    private void Show()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, yOffset, rectTransform.position.z);

        rectTransform.localScale = scale;
    }
    private void Hide()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, yStart, rectTransform.position.z);
        rectTransform.localScale = startScale;
    }
}
