using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUIContainer : MonoBehaviour
{
    public Grid mapGrid;

    public CountryUIItem defaultCountryItem;
    public ProvinceUIItem defaultProvinceItem;

    public IEnumerable<CountryUIItem> allCountryItem => uiItems.OfType<CountryUIItem>();
    public IEnumerable<ProvinceUIItem> allProvinceItem => uiItems.OfType<ProvinceUIItem>();

    private List<MapUIItem> uiItems { get; } = new List<MapUIItem>();

    internal void SetProvinces(IEnumerable<IProvince> provinces)
    {
        foreach (var province in provinces)
        {
            var item = Instantiate<ProvinceUIItem>(defaultProvinceItem, defaultProvinceItem.transform.parent);
            item.gmData = province;
            item.cellPos = HuangD.Maps.Utilty.GetCenterPos(province.block.elements);

            item.gameObject.SetActive(true);
            uiItems.Add(item);
        }
    }

    internal void UpdateItemsPosition()
    {
        foreach (var elem in uiItems)
        {
            var worldPos = mapGrid.CellToWorld(new Vector3Int(elem.cellPos.x, elem.cellPos.y));

            elem.transform.position = worldPos;
        }
    }

    internal void SetCountries(IEnumerable<ICountry> countries)
    {
        foreach(var country in countries)
        {
            var item = Instantiate<CountryUIItem>(defaultCountryItem, defaultCountryItem.transform.parent);
            item.gmData = country;
            item.cellPos = HuangD.Maps.Utilty.GetCenterPos(country.provinces.SelectMany(x=>x.block.elements));

            item.gameObject.SetActive(true);
            uiItems.Add(item);
        }
    }

    void Start()
    {
        defaultCountryItem.gameObject.SetActive(false);
        defaultProvinceItem.gameObject.SetActive(false);

        UpdateItemsPosition();
    }
}
