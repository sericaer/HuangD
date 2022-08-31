using HuangD.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceMapUI : MonoBehaviour
{
    public Text label;

    public (int x, int y) cellPos { get; set; }

    public IProvince gmData
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


    private IProvince _gmData;
}