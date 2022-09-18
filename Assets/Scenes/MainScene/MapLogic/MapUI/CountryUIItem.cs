using HuangD.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountryUIItem : MapUIItem
{
    public Text label;


    public ICountry gmData
    {
        get
        {
            return _gmData;
        }
        set
        {
            _gmData = value;
            label.text = _gmData.name;
            name = _gmData.name;
        }
    }


    private ICountry _gmData;

    private void Start()
    {

    }

    internal void SetAlpha(float alpha)
    {
        label.GetComponent<CanvasRenderer>().SetAlpha(alpha);
    }
}
