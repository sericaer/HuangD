using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUIContainer : MonoBehaviour, IEnumerable<MapUIItem>
{
    public CountryUIItem defaultCountryItem;
    public ProvinceUIItem defaultProvinceItem;

    public IEnumerable<CountryUIItem> allCountryItem => uiItems.OfType<CountryUIItem>();
    public IEnumerable<ProvinceUIItem> allProvinceItem => uiItems.OfType<ProvinceUIItem>();

    private List<MapUIItem> uiItems { get; } = new List<MapUIItem>();

    public IEnumerator<MapUIItem> GetEnumerator()
    {
        return ((IEnumerable<MapUIItem>)uiItems).GetEnumerator();
    }

    internal void SetCountris(IEnumerable<ICountry> countries)
    {
        foreach (var country in countries)
        {
            var item = Instantiate(defaultCountryItem, defaultCountryItem.transform.parent);
            item.gmData = country;
            item.cellPos = HuangD.Maps.Utilty.GetCenterPos(country.provinces.SelectMany(x=>x.cells).Select(x => x.position));

            item.gameObject.SetActive(true);
            uiItems.Add(item);
        }
    }

    internal void SetProvinces(IEnumerable<IProvince> provinces)
    {
        foreach (var province in provinces)
        {
            var item = Instantiate(defaultProvinceItem, defaultProvinceItem.transform.parent);
            item.gmData = province;
            item.cellPos = HuangD.Maps.Utilty.GetCenterPos(province.cells.Select(x => x.position));

            item.gameObject.SetActive(true);
            uiItems.Add(item);
        }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)uiItems).GetEnumerator();
    }

    void Start()
    {
        defaultCountryItem.gameObject.SetActive(false);
        defaultProvinceItem.gameObject.SetActive(false);
    }
}