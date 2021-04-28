using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetManaValue : MonoBehaviour
{
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
    private void ChangeValue(float amount, int currentAmount)
    {
        if (!manaSlider) //As it's called from the awake method this avoids 
            Init();

        manaSlider.value = amount;
        manaText.text = currentAmount.ToString();
    }
    private void Init()
    {
        manaSlider = GetComponent<Slider>();
        manaText = GetComponentInChildren<Text>();
    }
}
