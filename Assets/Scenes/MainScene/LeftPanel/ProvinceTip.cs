using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ProvinceTip : UIBehaviour<IProvince>
{
    public Text provinceName;
    public Text population;
    public Text landCount;

    public LandChart landChart;

    private Dictionary<BiomeType, List<ICell>> biome2Cells;

    protected override void AssocDataSource()
    {
        biome2Cells = new Dictionary<BiomeType, List<ICell>>();

        Bind(province => province.name, provinceName);
        Bind(province => province.population, population);
        Bind(province => province.cells.Count(), landCount);
        Bind(province => GroupByBiomeType(province.cells), landChart);
    }

    public IEnumerable<List<ICell>> GroupByBiomeType(IEnumerable<ICell> cells)
    {
        foreach (var list in biome2Cells.Values)
        {
            list.Clear();
        }

        foreach (var cell in cells)
        {
            if (!biome2Cells.ContainsKey(cell.landInfo.biome))
            {
                biome2Cells.Add(cell.landInfo.biome, new List<ICell>() { cell });
            }
            else
            {
                biome2Cells[cell.landInfo.biome].Add(cell);
            }
        }

        return biome2Cells.Values.Where(v => v.Count > 0);
    }
}