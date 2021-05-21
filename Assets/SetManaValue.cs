using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetManaValue : MonoBehaviour
{
    public GameObject whiskas;
    public GameObject maxWhiskas;

    private Image[] whiskas_array;
    private Image[] disactiveWhiskas_array;

    private Slider manaSlider;
    private Text manaText;

    private void OnEnable()
    {
        TurnManager.setDisplay += ChangeValue;
    }
    private void OnDisable()
    {
        TurnManager.setDisplay -= ChangeValue;
    }
    private void ChangeValue(int currentAmount, int maxMana)
    {
        if (whiskas_array == null) //As it's called from the awake method this avoids 
            Init();

        for (int i = 0; i < currentAmount; i++)
        {
            whiskas_array[i].gameObject.SetActive(true);
        }
        for (int i = currentAmount; i < whiskas_array.Length; i++)
        {
            whiskas_array[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < maxMana; i++)
        {
            disactiveWhiskas_array[i].gameObject.SetActive(true);
        }
        //manaSlider.value = amount;
        manaText.text = currentAmount.ToString();
    }
    private void Init()
    {
        //manaSlider = GetComponent<Slider>();
        manaText = GetComponentInChildren<Text>();
        whiskas_array = whiskas.GetComponentsInChildren<Image>();
        disactiveWhiskas_array = maxWhiskas.GetComponentsInChildren<Image>();

        for (int i = 0; i < disactiveWhiskas_array.Length; i++)
        {
            disactiveWhiskas_array[i].gameObject.SetActive(false);
        }
    }
}
