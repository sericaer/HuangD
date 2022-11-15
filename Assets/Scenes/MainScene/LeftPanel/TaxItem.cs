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
        Bind(income => income.GetType().Name, label);

        Bind(income => income.currValue, value, income => 
        {
            return string.Join("\n", income.effects.Select(x => $"{x.value*100}% {x.from}").Append($"baseValue {income.baseValue}").Reverse());
        });

        BindTwoWay(income => income.level, levels, (income, level) =>
        {
            var effects = income.GetLevelEffects(level);

            return string.Join("\n", effects.Select(x => $"{x.factor} {x.target}"));
        });
    }
}
