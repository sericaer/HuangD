using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandChart : UICollectionBehaviour<List<ICell>>
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (var item in uiContainer.GetComponentsInChildren<LandChartItem>()
                    .Where(x => x != defaultUIItem)
                    .OrderBy(x => int.Parse(x.itemValue.text))
                    .ThenBy(x => x.itemName.text))
        {
            item.transform.SetAsFirstSibling();
        }
    }
}
