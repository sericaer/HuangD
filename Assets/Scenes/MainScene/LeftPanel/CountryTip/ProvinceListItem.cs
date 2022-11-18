using HuangD.Interfaces;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProvinceListItem : UIBehaviour<IProvince>
{
    public Text provinceName;
    public Text population;
    public Text landCount;
    public Text liveliHood;
    public Text tax;

    public Button button;

    public UnityEvent<IProvince> ShowProvince;

    protected override void Awake()
    {
        base.Awake();

        button.onClick.AddListener(() =>
        {
            ShowProvince.Invoke(dataSource);
        });
    }
    protected override void AssocDataSource()
    {
        Bind(province => province.name, provinceName);
        Bind(province => province.pop.count.currValue, population);
        Bind(province => province.cells.Count(), landCount);
        Bind(province => province.pop.liveliHood.level.title, liveliHood);
        Bind(province => province.taxItems.Sum(x=>x.currValue), tax);
    }
}