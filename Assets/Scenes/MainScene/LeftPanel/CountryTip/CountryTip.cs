using HuangD.Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountryTip : UIBehaviour<ICountry>
{
    public Text countryName;
    public Text population;
    public Text provinceCount;
    public ProvinceList provinceList;

    protected override void AssocDataSource()
    {
        Bind(country => country.name, countryName);
        Bind(country => country.provinces.Sum(p=>p.pop.count.currValue), population);
        Bind(country => country.provinces.Count(), provinceCount);
        Bind(country => country.provinces, provinceList);
    }
}
