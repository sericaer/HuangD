using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilitaryPanel : UIBehaviour<IMilitary>
{
    public Text current;
    public Text max;
    public NumberText Inc;

    protected override void AssocDataSource()
    {
        Bind(military => military.currValue, current);
        Bind(military => military.maxValue, max);
        Bind(military => military.incValue, Inc);
    }
}
