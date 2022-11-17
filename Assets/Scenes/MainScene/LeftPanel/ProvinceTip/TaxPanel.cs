using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaxPanel : UIBehaviour<IEnumerable<ITreasury.IIncomeItem>>
{
    public Text taxValue;
    public TaxDetails taxDetails;

    protected override void AssocDataSource()
    {
        Bind(taxItems => taxItems.Sum(t => t.currValue), taxValue);
        Bind(taxItems => taxItems, taxDetails);
    }
}
