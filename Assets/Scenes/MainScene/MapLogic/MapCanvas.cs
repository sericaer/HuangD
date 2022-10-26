using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCanvas : MonoBehaviour, IPointerDownHandler
{
    //public Grid mapGrid;

    public Grid mapGrid;

    public MapRender mapRender;
    public MapCamera mapCamera;
    public MapUIContainer mapUIContainer;

    private IMap mapData;

    public void SetMapData(ISession session)
    {
        mapData = session.map;

        mapRender.SetData(session.map);
        mapRender.SetPliticalMap(session.provinces, session.countries);

        mapUIContainer.SetProvinces(session.provinces);

        var center = GetMapCenterPosition();
        mapCamera.MoveTo(new Vector3(center.x, center.y, mapCamera.transform.position.z));
    }

    public void OnCameraMoved()
    {
        foreach (var item in mapUIContainer)
        {
            item.transform.position = mapGrid.CellToWorld(new Vector3Int(item.cellPos.x, item.cellPos.y));
        }
    }

    void Start()
    {
        foreach (var item in mapUIContainer)
        {
            item.transform.position = mapGrid.CellToWorld(new Vector3Int(item.cellPos.x, item.cellPos.y));
        }
    }

    //internal void SetProvinces(IEnumerable<IProvince> provinces)
    //{
    //    foreach (var province in provinces)
    //    {
    //        var item = Instantiate<ProvinceUIItem>(defaultProvinceItem, defaultProvinceItem.transform.parent);
    //        item.gmData = province;
    //        item.cellPos = HuangD.Maps.Utilty.GetCenterPos(province.cells.Select(x=>x.position));

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

    //void Start()
    //{
    //    defaultCountryItem.gameObject.SetActive(false);
    //    defaultProvinceItem.gameObject.SetActive(false);

    //    UpdateItemsPosition();
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");

        var cellIndex = mapGrid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
        var pos = (cellIndex.x, cellIndex.y);

        var block = mapData.blockMap[pos];

        Debug.Log($"POS:{pos}, Height:{block.height}, terrain:{block.terrain}, rain:{block.rain}, wetness:{block.wetness}, biomes:{block.landInfo?.biome}， population{block.landInfo?.population}");
    }

    //private void MoveCameraToMapCenter()
    //{
    //    var positions = mapData.blockMap.Select(x => x.position);

    //    var maxX = positions.Max(p => p.x);
    //    var maxY = positions.Max(p => p.y);
    //    var minX = positions.Min(p => p.x);
    //    var minY = positions.Min(p => p.y);

    //    var mapCenterPos = mapGrid.CellToWorld(new Vector3Int((maxX - minX) / 2, (maxY - minY) / 2));
    //    mapCamera.transform.position = new Vector3(mapCenterPos.x, mapCenterPos.y, mapCamera.transform.position.z);
    //}

    private Vector3 GetMapCenterPosition()
    {
        var positions = mapData.blockMap.Select(x => x.position);

        var maxX = positions.Max(p => p.x);
        var maxY = positions.Max(p => p.y);
        var minX = positions.Min(p => p.x);
        var minY = positions.Min(p => p.y);

        return mapGrid.CellToWorld(new Vector3Int((maxX - minX) / 2, (maxY - minY) / 2));
    }
}
