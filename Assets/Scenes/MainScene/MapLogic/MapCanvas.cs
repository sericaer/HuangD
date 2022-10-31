using HuangD.Interfaces;
using HuangD.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MapCanvas : MonoBehaviour, IPointerDownHandler
{

    public Grid mapGrid;

    public MapRender mapRender;
    public MapCamera mapCamera;
    public MapUIContainer mapUIContainer;

    private IMap mapData;

    public UnityEvent<object> politcalMapSelected;

    public void SetMapData(ISession session)
    {
        mapData = session.map;

        mapRender.SetGeographMap(session.map);
        mapRender.SetPliticalMap(session.provinces, session.countries);

        mapUIContainer.SetProvinces(session.provinces);
        mapUIContainer.SetCountris(session.countries);

        moveCameraTo(session.playerCountry);

    }

    internal void moveCameraTo(ICountry country)
    {
        var startCellIndex = Utilty.GetCenterPos(country.provinces.SelectMany(x => x.cells)
            .Select(x => x.position));

        var startPos = mapGrid.CellToWorld(new Vector3Int(startCellIndex.x, startCellIndex.y));

        mapCamera.MoveTo(new Vector3(startPos.x, startPos.y, mapCamera.transform.position.z));
    }

    public void OnCameraMoved()
    {
        foreach (var item in mapUIContainer)
        {
            item.transform.position = mapGrid.CellToWorld(new Vector3Int(item.cellPos.x, item.cellPos.y));
        }
    }
    public void OnCameraZoom(float size)
    {
        var alpha = Mathf.Min(size * 2, 1f);

        foreach (var item in mapUIContainer.allCountryItem)
        {
            item.SetAlpha(alpha);
            mapRender.countryMap.SetAlpha(alpha);
        }

        foreach (var item in mapUIContainer.allProvinceItem)
        {
            item.SetAlpha(1 - alpha);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");

        var cellIndex = mapGrid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
        var pos = (cellIndex.x, cellIndex.y);

        var block = mapData.blockMap[pos];

        Debug.Log($"POS:{pos}, Height:{block.height}, terrain:{block.terrain}, rain:{block.rain}, wetness:{block.wetness}, biomes:{block.landInfo?.biome}, population{block.landInfo?.population}, province{block.province?.name}, country{block.province?.country.name}");

        if(mapRender.countryMap.GetAlpha() > 0.8)
        {
            politcalMapSelected.Invoke(block.province?.country);
        }
        else
        {
            politcalMapSelected.Invoke(block.province);
        }
    }

    void Start()
    {

    }
}
