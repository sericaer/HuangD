﻿using HuangD.Interfaces;
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
        Bind(income => income.currValue, value);
        BindTwoWay(income => income.level, levels);

        AddToolTip(value, income =>
        {
            return string.Join("\n", income.effects.Select(x => (x.value > 0 ? "+" : "") + $"{x.value * 100}% {x.from.title}").Append($"基础值 {income.baseValue}").Reverse());
        });

        AddTooltip<ITreasury.CollectLevel>(levels, (income, level) =>
        {
            var effects = income.GetLevelEffects(level);

            return string.Join("\n", effects.Select(x => $"{x.factor} {x.target}"));
        });
    }
}