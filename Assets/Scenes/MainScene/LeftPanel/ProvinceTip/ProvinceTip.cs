using HuangD.Interfaces;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProvinceTip : UIBehaviour<IProvince>
{
    public Text provinceName;
    public Button country;

    public PopPanel popPanel;
    public LandPanel landPanel;
    public TaxPanel taxPanel;
    public MilitaryProvPanel militaryPanel;

    public UnityEvent<ICountry> ShowCountry;

    protected override void Awake()
    {
        country.onClick.AddListener(()=>
        {
            ShowCountry.Invoke(dataSource.country);
        });
    }

    protected override void AssocDataSource()
    {
        Bind(province => province.name, provinceName);
        Bind(province => province.country.name, country.GetComponentInChildren<Text>());

        popPanel.dataSource = dataSource.pop;
        landPanel.dataSource = dataSource.cells;
        taxPanel.dataSource = dataSource.taxItems;
        militaryPanel.dataSource = dataSource.militaryItems;

        //AddToolTip(liveliHood, province =>
        //{
        //    return string.Join("\n", province.pop.liveliHood.details.Select(x => (x.value > 0 ? "+" : "") + $"{x.value} {x.from.title}").Prepend($"����ֵ {province.pop.liveliHood.baseInc}"));
        //});
    }
}
