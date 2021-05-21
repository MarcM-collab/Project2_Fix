using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetHealthText : MonoBehaviour
{
    private Character parentChar;
    private TMP_Text text;
    void Start()
    {
        parentChar = GetComponentInParent<Character>();
        text = GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        text.text = parentChar.HP.ToString();
    }
}
