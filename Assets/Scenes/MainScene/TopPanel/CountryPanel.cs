using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryPanel : MonoBehaviour
{
    public Text label;

    internal void SetCountry(ICountry country)
    {
        label.text = country.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
