using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    private Entity _entity;
    public GameObject EntityGameObject;
    public Color color;
    public Image FillImage;
    // Start is called before the first frame update
    void Awake()
    {
        _entity = EntityGameObject.GetComponent<Entity>();
        _slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        _entity.OnChangeHealth += SetValue;
    }
    private void OnDisable()
    {
        _entity.OnChangeHealth -= SetValue;
    }

    public void SetValue(float f)
    {
        _slider.value = f;
        FillImage.color = color; //should be setted in spawn
    }
}
