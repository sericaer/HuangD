using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberText : MonoBehaviour
{
    public int decimals;
    public bool showSign;

    public Color PositiveColor;
    public Color NegativeColor;

    [SerializeField]
    public double Value
    {
        get
        {
            return _Value;
        }
        set
        {
            _Value = value;
            SetNumberValue(Value);
        }
    }

    [SerializeField]
    private double _Value;
    private Text text => GetComponent<Text>();

    private void SetNumberValue(double Value)
    {
        text.text = System.Math.Round(Value, decimals).ToString();

        if (showSign && !(Value < 0))
        {
            text.text = "+" + text.text;
        }


        if (Value < 0)
        {
            text.color = NegativeColor;
        }
        else
        {
            text.color = PositiveColor;
        }
    }
}