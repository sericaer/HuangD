using HuangD.Interfaces;
using System.Linq;
using UnityEngine.UI;

public class TaxItem : UIBehaviour<ITreasury.IIncomeItem>
{
    public Text label;
    public Text value;
    public ToggleGroupEx levels;

    protected override void AssocDataSource()
    {
        Bind(income => income.type, label);

        Bind(income => income.currValue, value, income => 
        {
            return string.Join("\n", income.effects.Select(x => $"{x.value} {x.from}").Append($"baseValue {income.baseValue}").Reverse());
        });

        BindTwoWay(income => income.level, levels);
    }
}
