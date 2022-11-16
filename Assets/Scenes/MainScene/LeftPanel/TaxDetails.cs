using HuangD.Interfaces;
using System;
using System.Linq;

public class TaxDetails : UICollectionBehaviour<ITreasury.IIncomeItem, TaxItem>
{
    internal void SetInteractive(bool flag)
    {
        var items = uiContainer.GetComponentsInChildren<TaxItem>();
        foreach (var toggle in items.SelectMany(x=>x.levels.toggles))
        {
            toggle.interactable = flag;
        }
    }
}