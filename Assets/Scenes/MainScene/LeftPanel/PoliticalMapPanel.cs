using HuangD.Interfaces;
using UnityEngine;

public class PoliticalMapPanel : MonoBehaviour
{
    public ProvinceTip provinceTip;
    public CountryTip countryTip;

    public void OnShow(object obj)
    {
        switch (obj)
        {
            case IProvince province:
                countryTip.OnShow(null);
                provinceTip.OnShow(province);
                break;
            case ICountry country:
                provinceTip.OnShow(null);
                countryTip.OnShow(country);
                break;
            default:
                countryTip.OnShow(null);
                provinceTip.OnShow(null);
                break;
        }
    }
}
