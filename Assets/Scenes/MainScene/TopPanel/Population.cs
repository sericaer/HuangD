using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Population : UIBehaviour<ICountry>
{
    public Text count;
    public IncFlag incFlag;
    protected override void AssocDataSource()
    {
        Bind(country => $"{System.Math.Round(country.provinces.Sum(p => p.pop.count.currValue) / 1000.0, 1)}K", count);
        Bind(country => country.provinces.Sum(p => p.pop.count.currInc) > 0, incFlag);
    }
}
