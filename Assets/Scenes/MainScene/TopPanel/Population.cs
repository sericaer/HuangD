using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Population : UIBehaviour<ICountry>
{
    public Text count;

    protected override void AssocDataSource()
    {
        Bind(country => country.provinces.Sum(p => p.pop.count.currValue), count);
    }
}
