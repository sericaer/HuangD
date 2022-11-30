using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasuryDetailPanel : UIBehaviour<ITreasury>
{
    public Text totalIncome;
    public Text totalSpend;
    public Text surplus;

    public PieChartExt incomeChart;
    public PieChartExt spendChart;

    protected override void AssocDataSource()
    {
        Bind(treasury => treasury.income, totalIncome);
        Bind(treasury => treasury.spend, totalSpend);
        Bind(treasury => treasury.surplus, surplus);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach(var item in dataSource.incomeItems)
        {
            incomeChart.AddOrUpdate(item.key, item.currValue);
        }

        foreach (var item in dataSource.spendItems)
        {
            spendChart.AddOrUpdate(item.key, item.currValue);
        }
    }
}
