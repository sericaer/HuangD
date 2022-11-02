using HuangD.Interfaces;
using UnityEngine;

public class PoliticalMapPanel : MonoBehaviour
{
    public ProvinceTip provinceTip;
    public CountryTip countryTip;

    private void Start()
    {
        provinceTip.gameObject.SetActive(false);
        countryTip.gameObject.SetActive(false);
    }

    public void OnShow(object obj)
    {
        switch (obj)
        {
            case IProvince province:
                provinceTip.dataSource = province;
                provinceTip.gameObject.SetActive(true);
                break;
            case ICountry country:
                countryTip.OnShow(country);
                break;
            default:
                provinceTip.gameObject.SetActive(false);
                countryTip.gameObject.SetActive(false);
                break;
        }
    }
}
