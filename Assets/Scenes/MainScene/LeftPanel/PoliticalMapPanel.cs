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
        provinceTip.gameObject.SetActive(false);
        countryTip.gameObject.SetActive(false);

        switch (obj)
        {
            case IProvince province:
                provinceTip.dataSource = province;
                provinceTip.gameObject.SetActive(true);
                provinceTip.transform.SetAsLastSibling();
                break;
            case ICountry country:
                countryTip.dataSource = country;
                countryTip.gameObject.SetActive(true);
                provinceTip.transform.SetAsLastSibling();
                break;
            default:
                throw new System.Exception();
        }
    }
}
