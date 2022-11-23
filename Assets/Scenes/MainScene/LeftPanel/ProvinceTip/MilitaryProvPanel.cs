using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MilitaryProvPanel : UIBehaviour<IEnumerable<IMilitary.IItem>>
{
    public Text taxValue;
    public MilitaryDetails militaryDetails;

    protected override void AssocDataSource()
    {
        Bind(items => items.Sum(t => t.currValue), taxValue);
        Bind(items => items, militaryDetails);
    }
}
