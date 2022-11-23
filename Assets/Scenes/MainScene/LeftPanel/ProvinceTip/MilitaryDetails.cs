using HuangD.Interfaces;
using System.Linq;

public class MilitaryDetails : UICollectionBehaviour<IMilitary.IItem, MilitaryItem>
{
    internal void SetInteractive(bool flag)
    {
        var items = uiContainer.GetComponentsInChildren<TaxItem>();
        foreach (var toggle in items.SelectMany(x => x.levels.toggles))
        {
            toggle.interactable = flag;
        }
    }
}