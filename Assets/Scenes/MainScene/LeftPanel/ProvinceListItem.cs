using HuangD.Interfaces;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProvinceListItem : UIBehaviour<IProvince>
{
    public Text provinceName;
    public Text population;
    public Text landCount;
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
        Bind(province => province.pop.count, population);
        Bind(province => province.cells.Count(), landCount);
    }
}