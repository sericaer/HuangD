using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CountryPanel : MonoBehaviour
{
    public Text label;

    public LeaderPanel leaderPanel;
    public TreasuryPanel treasuryPanel;
    public Population population;
    public ProvinceCountPanel ProvinceCountPanel;

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

            FixedUpdate();
        }
    }

    private ICountry _country;

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
