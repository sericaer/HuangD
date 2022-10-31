using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.Linq;

public class PieChartTest : MonoBehaviour
{
    //public PieChart pieChart;

    //List<SerieData> serieDatas = new List<SerieData>();

    //int count = 0;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    pieChart.ClearData();

    //    pieChart.AddData(0, 1, null, "test1");
    //    pieChart.AddData(0, 1, null, "test2");
    //}

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    pieChart.UpdateData(0, 0, Random.Range(1,10));
    //    pieChart.UpdateData(0, 1, Random.Range(1, 10));

    //    count++;
    //}

    public PieChartExt pieChartExt;

    void Start()
    {
        pieChartExt.AddOrUpdate("test1", 1);
        pieChartExt.AddOrUpdate("test2", 1);
    }

    void FixedUpdate()
    {
        pieChartExt.AddOrUpdate("test1", Random.Range(1, 10));
        pieChartExt.AddOrUpdate("test2", Random.Range(1, 10));
    }
}
