using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCanvas : MonoBehaviour, IPointerDownHandler
{
    //public Grid mapGrid;

    //public CountryUIItem defaultCountryItem;
    //public ProvinceUIItem defaultProvinceItem;

    //public IEnumerable<CountryUIItem> allCountryItem => uiItems.OfType<CountryUIItem>();
    //public IEnumerable<ProvinceUIItem> allProvinceItem => uiItems.OfType<ProvinceUIItem>();

    //private List<MapUIItem> uiItems { get; } = new List<MapUIItem>();

    public Grid mapGrid;

    public MapRender mapRender;
    public MapCamera mapCamera;

    private IMap mapData;

    public void SetMapData(IMap map)
    {
        mapData = map;

        mapRender.SetData(map);

        MoveCameraToMapCenter();

        //throw new NotImplementedException();
    }

    //internal void SetProvinces(IEnumerable<IProvince> provinces)
    //{
    //    foreach (var province in provinces)
    //    {
    //        var item = Instantiate<ProvinceUIItem>(defaultProvinceItem, defaultProvinceItem.transform.parent);
    //        item.gmData = province;
    //        item.cellPos = HuangD.Maps.Utilty.GetCenterPos(province.block.elements);

    //        item.gameObject.SetActive(true);
    //        uiItems.Add(item);
    //    }
    //}


    //internal void UpdateItemsPosition()
    //{
    //    foreach (var elem in uiItems)
    //    {
    //        var worldPos = mapGrid.CellToWorld(new Vector3Int(elem.cellPos.x, elem.cellPos.y));

    //        elem.transform.position = worldPos;
    //    }
    //}

    //internal void SetCountries(IEnumerable<ICountry> countries)
    //{
    //    foreach(var country in countries)
    //    {
    //        var item = Instantiate<CountryUIItem>(defaultCountryItem, defaultCountryItem.transform.parent);
    //        item.gmData = country;
    //        item.cellPos = HuangD.Maps.Utilty.GetCenterPos(country.provinces.SelectMany(x=>x.block.elements));

    //        item.gameObject.SetActive(true);
    //        uiItems.Add(item);
    //    }
    //}

    void Start()
    {
        //defaultCountryItem.gameObject.SetActive(false);
        //defaultProvinceItem.gameObject.SetActive(false);

        //UpdateItemsPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");

        var cellIndex = mapGrid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
        var pos = (cellIndex.x, cellIndex.y);

        Debug.Log($"POS:{pos}, Height:{mapData.heightMap[pos]}, terrain:{mapData.terrains[pos]}, rain:{mapData.rainMap[pos]}, wetness:{mapData.wetnessMap[pos]}, biomes:{mapData.biomesMap[pos]}， population{mapData.populationMap[pos]}");
    }

    private void MoveCameraToMapCenter()
    {
        var positions = mapData.nosieMap.Select(x => x.Key);

        var maxX = positions.Max(p => p.x);
        var maxY = positions.Max(p => p.y);
        var minX = positions.Min(p => p.x);
        var minY = positions.Min(p => p.y);

        var mapCenterPos = mapGrid.CellToWorld(new Vector3Int((maxX - minX) / 2, (maxY - minY) / 2));
        mapCamera.transform.position = new Vector3(mapCenterPos.x, mapCenterPos.y, mapCamera.transform.position.z);
    }
}
