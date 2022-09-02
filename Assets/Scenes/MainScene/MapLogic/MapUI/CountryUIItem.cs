using HuangD.Interfaces;
using UnityEngine.UI;

public class CountryUIItem : MapUIItem
{
    public Text label;


    public ICountry gmData
    {
        get
        {
            return _gmData;
        }
        set
        {
            _gmData = value;
            label.text = _gmData.name;
        }
    }


    private ICountry _gmData;
}
