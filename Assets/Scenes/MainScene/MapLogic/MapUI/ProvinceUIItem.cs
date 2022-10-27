using HuangD.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceUIItem : MapUIItem
{
    public Text label;

    public IProvince gmData
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

    private IProvince _gmData;
}