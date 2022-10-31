using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.Linq;
using System;

public class PieChartExt : MonoBehaviour
{
    public PieChart pieChart;
    public LegendItem defaultLegendItem;

    private Dictionary<string, double> dict = new Dictionary<string, double>();

    void Start()
    {
        defaultLegendItem.gameObject.SetActive(false);

        pieChart.ClearData();

        var needRemoves = defaultLegendItem.transform.parent.GetComponentsInChildren<LegendItem>()
            .Where(x => x != defaultLegendItem).ToArray();

        foreach(var elem in needRemoves)
        {
            Destroy(elem.gameObject);
        }
        
        foreach (var pair in dict)
        {
            pieChart.AddData(0, pair.Value, pair.Key, pair.Key);

            InstanceNewItem(pair.Key, pair.Value);
        }

        UpdateLegendsOrder();
    }

    public void AddOrUpdate(string key, double value)
    {
        dict[key] = value;

        if(pieChart.GetAllSerieDataCount() == 0)
        {
            pieChart.AddData(0, value, key, key);
            return;
        }

        var seriaData = pieChart.series[0].GetSerieData(key);
        if (seriaData == null)
        {
            pieChart.AddData(0, value, key, key);
            return;
        }

        pieChart.series[0].UpdateData(seriaData.index, 1, value);

        var item = GetComponentsInChildren<LegendItem>().SingleOrDefault(x=>x.ItemName.text == key);
        if(item == null)
        {
            item = InstanceNewItem(key, value);
        }

        item.ItemValue.text = value.ToString();

        UpdateLegendsOrder();
    }

    private void UpdateLegendsOrder()
    {
        var items = defaultLegendItem.transform.parent.GetComponentsInChildren<LegendItem>()
            .Where(x => x != defaultLegendItem)
            .OrderByDescending(x=> double.Parse(x.ItemValue.text))
            .ToArray();

        foreach (var item in items)
        {
            item.gameObject.transform.SetAsFirstSibling();
        }
    }

    private LegendItem InstanceNewItem(string key, double value)
    {
        LegendItem item = Instantiate<LegendItem>(defaultLegendItem, defaultLegendItem.transform.parent);
        item.ItemName.text = key;
        item.ItemValue.text = value.ToString();
        item.gameObject.SetActive(true);

        return item;
    }

    public void RemoveKey(string key)
    {
        dict.Remove(key);

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
}