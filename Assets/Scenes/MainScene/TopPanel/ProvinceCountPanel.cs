using HuangD.Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceCountPanel : UIBehaviour<ICountry>
{
    public Text label;

    protected override void AssocDataSource()
    {
        Bind(country => country.provinces.Count(), label);
    }
}