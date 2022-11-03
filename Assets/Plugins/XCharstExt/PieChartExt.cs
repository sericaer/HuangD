using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.Linq;
using System;

public class PieChartExt : MonoBehaviour
{
    public PieChart pieChart;

    void Start()
    {

        pieChart.ClearData();
    }

    public void AddOrUpdate(string key, double value, Color? color = null)
    {
        var serieData = AddOrUpdateSerieData(key, value);

        if(color != null)
        {
            var itemStyle = serieData.GetOrAddComponent<ItemStyle>(); //给数据项添加ItemStyle组件
            itemStyle.color = color.Value;
        }
    }

    private SerieData AddOrUpdateSerieData(string key, double value)
    {
        if (pieChart.GetAllSerieDataCount() == 0)
        {
            return pieChart.AddData(0, value, key, key);
        }

        var seriaData = pieChart.series[0].GetSerieData(key);
        if (seriaData == null)
        {
            return pieChart.AddData(0, value, key, key);
        }

        seriaData.UpdateData(1, value);
        return seriaData;
    }

    public void RemoveKey(string key)
    {

        if (pieChart.GetAllSerieDataCount() == 0)
        {
            return;
        }

        var seriaData = pieChart.series[0].GetSerieData(key);
        if (seriaData == null)
        {
            return;
        }

        pieChart.series[0].RemoveData(seriaData.index);

    }

    public void Clear()
    {
        pieChart.ClearData();
    }
}