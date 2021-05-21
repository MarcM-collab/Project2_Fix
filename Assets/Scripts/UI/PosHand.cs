using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosHand : MonoBehaviour
{
    public Vector3 scale;
    private Vector3 startScale;
    private RectTransform rectTransform;
    public float yOffset = -302.8f;
    private float yStart;
    [HideInInspector] public bool shown = false;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startScale = rectTransform.localScale;
        yStart = rectTransform.position.y;
    }
    public void Show()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, yOffset, rectTransform.position.z);
        rectTransform.localScale = scale;

        shown = true;
    }
    public void Hide()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, yStart, rectTransform.position.z);
        rectTransform.localScale = startScale;

        shown = false;
    }
}
