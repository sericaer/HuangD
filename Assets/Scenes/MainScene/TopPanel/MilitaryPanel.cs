using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilitaryPanel : UIBehaviour<IMilitary>
{
    public Text current;
    public IncFlag incFlag;

    protected override void AssocDataSource()
    {
        Bind(military => $"{System.Math.Round(military.currValue / 1000.0, 2)}K", current);
        Bind(military => military.incValue > 0, incFlag);
    }
}
