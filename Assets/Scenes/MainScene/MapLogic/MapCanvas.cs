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
        mapUIContainer.SetCountris(session.countries);

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

    void Start()
    {
        foreach (var item in mapUIContainer)
        {
            item.transform.position = mapGrid.CellToWorld(new Vector3Int(item.cellPos.x, item.cellPos.y));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");

        var cellIndex = mapGrid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
        var pos = (cellIndex.x, cellIndex.y);

        var block = mapData.blockMap[pos];

        Debug.Log($"POS:{pos}, Height:{block.height}, terrain:{block.terrain}, rain:{block.rain}, wetness:{block.wetness}, biomes:{block.landInfo?.biome}， population{block.landInfo?.population}");
    }

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
