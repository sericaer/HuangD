using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountryPanel : MonoBehaviour
{
    public Text label;

    public LeaderPanel leaderPanel;
    public TreasuryPanel treasuryPanel;
    public Population population;
    public ProvinceCountPanel ProvinceCountPanel;
    public MilitaryPanel militaryPanel;

    public UnityEvent<ICountry> ShowCountry;

    public ICountry country
    {
        get
        {
            return _country;
        }
        set
        {
            if(_country == value)
            {
                return;
            }

            _country = value;

            treasuryPanel.dataSource = _country.treasury;
            population.dataSource = _country;
            ProvinceCountPanel.dataSource = _country;
            militaryPanel.dataSource = _country.military;

            FixedUpdate();
        }
    }

    private ICountry _country;

    public void OnClick()
    {
        ShowCountry.Invoke(country);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        label.text = country.name;
        leaderPanel.leader = country.leader;
    }
}
