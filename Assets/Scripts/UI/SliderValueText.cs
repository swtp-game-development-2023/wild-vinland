using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    private Slider slider;

    private TMP_Text  textComp;

    private void Awake()
    {
        slider = GetComponentInParent<Slider>();
        textComp = GetComponent<TMP_Text>();
    }


    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }
    

    void UpdateText(float val)
    {
        textComp.text = slider.value.ToString("0.00");
    }
}
