using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProvinceTip : UIBehaviour<IProvince>
{
    public Text provinceName;
    public Text population;
    public Text liveliHood;
    public Text landCount;
    public Text taxValue;

    public Button country;

    public LandChart landChart;
    public TaxDetails taxDetails;

    public UnityEvent<ICountry> ShowCountry;

    private Dictionary<BiomeType, List<ICell>> biome2Cells;

    protected override void Awake()
    {
        country.onClick.AddListener(()=>
        {
            ShowCountry.Invoke(dataSource.country);
        });
    }

    protected override void AssocDataSource()
    {
        biome2Cells = new Dictionary<BiomeType, List<ICell>>();

        Bind(province => province.name, provinceName);
        Bind(province => province.pop.count, population);
        Bind(province => province.pop.liveliHood.currValue, liveliHood);
        Bind(province => province.cells.Count(), landCount);
        Bind(province => province.country.name, country.GetComponentInChildren<Text>());
        Bind(province => GroupByBiomeType(province.cells), landChart);
        Bind(province => province.taxItems.Sum(t=>t.currValue), taxValue);
        Bind(province => province.taxItems, taxDetails);

        AddToolTip(liveliHood, province =>
        {
            return string.Join("\n", province.pop.liveliHood.details.Select(x => $"{x.value} {x.from}").Prepend($"baseValue {province.pop.liveliHood.baseInc}"));
        });
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(taxDetails.gameObject.activeInHierarchy)
        {
            taxDetails.SetInteractive(dataSource.country == Facade.session.playerCountry);
        }
    }
}
