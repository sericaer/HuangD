using HuangD.Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CountryTip : MonoBehaviour
{
    public Text countryName;
    public Text population;
    public Text provinceCount;

    private ICountry country;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        countryName.text = country.name;
        population.text = country.provinces.Sum(x => x.population).ToString();
        provinceCount.text = country.provinces.Count().ToString();
    }

    public void OnShow(ICountry country)
    {
        this.country = country;
        this.gameObject.SetActive(this.country != null);
    }
}