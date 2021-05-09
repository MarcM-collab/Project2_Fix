using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUIShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool use = false;
    public GameObject toActive;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toActive && use)
            toActive.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (toActive && use)
            toActive.SetActive(false);
    }
}
