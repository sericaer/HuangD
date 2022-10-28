using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvincePanel : MonoBehaviour
{
    public Text provinceName;
    public Text population;
    public Text landCount;

    private IProvince province;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        provinceName.text = province.name;
        population.text = province.population.ToString();
        landCount.text = province.cells.Count().ToString();
    }

    public void OnProvinceShow(IProvince province)
    {
        this.province = province;
        this.gameObject.SetActive(this.province != null);
    }
}
