using HuangD.Interfaces;
using HuangD.Maps;
using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    public Camera mapCamera;
    public Grid mapGrid;

    public BlockMap blockMap;
    public EdgeMap edgeMap;
    public TerrainMap terrainMap;
    public CountryMap countryMap;
    public RiverMap riverMap;

    public MapUIContainer mapUIContainer;

    // Start is called before the first frame update
    void Start()
    {
        MoveCameraToMapCenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(Vector3 pos)
    {
        Vector3 move = CaclMoveOffset(pos);

        mapCamera.transform.position = mapCamera.transform.position + move;
        mapUIContainer.UpdateItemsPosition();
    }

    public void ScrollWheel(bool flag)
    {
        mapCamera.orthographicSize = CalcNextScale(flag);
        var alpha = (mapCamera.orthographicSize - 6) / (20 - 6);
        foreach (var item in mapUIContainer.allCountryItem)
        {
            item.SetAlpha(alpha);
            countryMap.SetAlpha(alpha);
        }

        foreach (var item in mapUIContainer.allProvinceItem)
        {
            item.SetAlpha(1-alpha);
        }

        mapUIContainer.UpdateItemsPosition();
    }

    internal void SetMapData(IMap map)
    {
        foreach (var pair in map.terrains)
        {
            terrainMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        }
        foreach (var pos in map.rivers.Keys)
        {
            riverMap.SetCell(new Vector3Int(pos.x, pos.y), map.rivers[pos]);
        }
    }

    private void MoveCameraToMapCenter()
    {
        var bound = terrainMap.tilemap.cellBounds;
        var mapCenterPos = mapGrid.CellToWorld(new Vector3Int((bound.xMax - bound.xMin) / 2, (bound.yMax - bound.yMin) / 2));
        mapCamera.transform.position = new Vector3(mapCenterPos.x, mapCenterPos.y, mapCamera.transform.position.z);
    }

    internal void SetProvinces(IEnumerable<IProvince> provinces)
    {
        foreach (var province in provinces)
        {
            var color = new Color(province.color.r, province.color.g, province.color.b);
            foreach (var pos in province.block.elements)
            {
                blockMap.SetCell(new Vector3Int(pos.x, pos.y), color);
            }
        }

        var edges = Utilty.GenerateEdges(provinces.Select(x => x.block.edges));
        foreach (var pair in edges)
        {
            edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value, EdgeMap.EdgeType.Province);
        }

        mapUIContainer.SetProvinces(provinces);
    }

    internal void SetCountries(IEnumerable<ICountry> countries)
    {
        foreach (var country in countries)
        {
            var color = new Color(country.color.r, country.color.g, country.color.b);
            foreach (var cellIndex in country.provinces.SelectMany(x => x.block.elements))
            {
                countryMap.SetCell(new Vector3Int(cellIndex.x, cellIndex.y), color);
            }
        }

        var countryEgdes = countries.Select(country =>
        {
            return country.provinces.SelectMany(prov => prov.block.edges)
                .Where(egde => Hexagon.GetNeighbors(egde).Any(n => country.provinces.All(x => !x.block.elements.Contains(n))));
          
        });

        var edges = Utilty.GenerateEdges(countryEgdes);
        foreach (var pair in edges)
        {
            edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value, EdgeMap.EdgeType.Country);
        }

        mapUIContainer.SetCountries(countries);
    }

    private Vector3 CaclMoveOffset(Vector3 pos)
    {
        Vector3 move = (pos - new Vector3(0.5f, 0.5f)) * 0.1f;
        Debug.Log(move);
        if (move.x < 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f));
            var cellIndex = terrainMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!terrainMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(0, move.y);
            }
        }
        else if (move.x > 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f));
            var cellIndex = terrainMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!terrainMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(0, move.y);
            }
        }

        if (move.y < 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f));
            var cellIndex = terrainMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!terrainMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(move.x, 0);
            }
        }
        else if (move.y > 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f));
            var cellIndex = terrainMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!terrainMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(move.x, 0);
            }
        }

        return move;
    }

    private float CalcNextScale(bool flag)
    {
        var center = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
        var cellIndex = terrainMap.tilemap.WorldToCell(center);
        if (!terrainMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
        {
            return mapCamera.orthographicSize;
        }

        var newSize = mapCamera.orthographicSize + 0.5f * (flag ? 1 : -1);
        if (newSize < 6f || newSize > 20f)
        {
            return mapCamera.orthographicSize;
        }

        return newSize;
    }
}
