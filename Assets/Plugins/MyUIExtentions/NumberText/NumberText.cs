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
    public string Format
    {
        get
        {
            return _Format;
        }
        set
        {
            _Format = value;
            SetNumberValue(Value);
        }
    }

    [SerializeField]
    private double _Value;

    [SerializeField]
    private string _Format;
    private Text text => GetComponent<Text>();

    private void SetNumberValue(double Value)
    {
        if(_Format  == null || _Format == "")
        {
            text.text = System.Math.Round(Value, decimals).ToString();
        }
        else
        {
            text.text = String.Format(Format, System.Math.Round(Value, decimals));
        }

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