using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LandChartItem : UIBehaviour<List<ICell>>
{
    public Text itemName;
    public Text itemValue;
    public Image image;

    public PieChartExt pieChart;

    public static Dictionary<BiomeType, Color> dictBiomeColor
    {
        get
        {
            if(_dictBiomeColor == null)
            {
                _dictBiomeColor = new Dictionary<BiomeType, Color>();

                Random.InitState(0);

                foreach(BiomeType biomeType in System.Enum.GetValues(typeof(BiomeType)))
                {
                    var color = Random.ColorHSV(0f, 1f, 0.8f, 0.8f, 0.8f, 1f);
                    while(_dictBiomeColor.Values.Any(c=> c == color))
                    {
                        color = Random.ColorHSV(0f, 1f, 0.8f, 0.8f, 0.8f, 1f);
                    }

                    _dictBiomeColor.Add(biomeType, color);
                }

                Random.InitState(Random.Range(0, 1));
            }

            return _dictBiomeColor;
        }
    }

    private static Dictionary<BiomeType, Color> _dictBiomeColor;


    protected override void AssocDataSource()
    {
        pieChart.Clear();

        image.color = dictBiomeColor[dataSource.First().landInfo.biome];

        Bind(dataSource => dataSource.First().landInfo.biome, itemName);
        Bind(dataSource => dataSource.Count(), itemValue);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var biomeType = dataSource.First().landInfo.biome;
        pieChart.AddOrUpdate(biomeType.ToString(), dataSource.Count(), dictBiomeColor[biomeType]);
    }

    protected override void OnDestroy()
    {
        if(dataSource != null)
        {
            pieChart.RemoveKey(dataSource.First().landInfo.biome.ToString());
        }

        base.OnDestroy();
    }
}
