using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceTip : MonoBehaviour
{
    public Text provinceName;
    public Text population;
    public Text landCount;

    public PieChartExt landPieChart;

    private IProvince province;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        provinceName.text = province.name;
        population.text = province.population.ToString();
        landCount.text = province.cells.Count().ToString();

        if(landPieChart.gameObject.activeInHierarchy)
        {
            foreach(var group in province.cells.GroupBy(x=>x.landInfo.biome))
            {
                landPieChart.AddOrUpdate(group.Key.ToString(), group.Count());
            }
        }
    }

    public void OnShow(IProvince province)
    {
        this.province = province;
        this.gameObject.SetActive(this.province != null);
    }
}
