using HuangD.Interfaces;
using HuangD.Maps;
using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    public BlockMap blockMap;
    public EdgeMap edgeMap;
    public TerrainMap terrainMap;
    public CountryMap countryMap;
    public RiverMap riverMap;
    public NoiseMap noiseMap;
    public RainMap rainMap;
    public HeightMap heightMap;
    public WetnessMap wetnessMap;
    public BiomesMap biomesMap;
    public PopulationMap populationMap;
    public ProvinceMap provinceMap;

    internal void SetData(IMap map)
    {
        foreach(var cell in map.blockMap)
        {
            noiseMap.SetCell(cell.position, cell.noise);
            blockMap.SetCell(cell.position, cell.block, cell.isBlockEdge);
            heightMap.SetCell(cell.position, cell.height);
            terrainMap.SetCell(cell.position, cell.terrain);
            rainMap.SetCell(cell.position, cell.rain);
            wetnessMap.SetCell(cell.position, cell.wetness);

            if(cell.landInfo != null)
            {
                biomesMap.SetCell(cell.position, cell.landInfo.biome);
                populationMap.SetCell(cell.position, cell.landInfo.population);
            }
        }

        foreach(var riverItem in map.riverMap)
        {
            riverMap.SetCell(riverItem.position, riverItem.index);
        }
    }

    internal void SetPliticalMap(IEnumerable<IProvince> provinces)
    {
        foreach (var province in provinces)
        {
            provinceMap.SetProvince(province);
        }
    }

    internal bool HasTile(Vector3 wordPos)
    {
        var cellIndex = noiseMap.tilemap.WorldToCell(wordPos);
        return noiseMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y));
    }

    //private void MoveCameraToMapCenter()
    //{
    //    var bound = terrainMap.tilemap.cellBounds;
    //    var mapCenterPos = mapGrid.CellToWorld(new Vector3Int((bound.xMax - bound.xMin) / 2, (bound.yMax - bound.yMin) / 2));
    //    mapCamera.transform.position = new Vector3(mapCenterPos.x, mapCenterPos.y, mapCamera.transform.position.z);
    //}

    //internal void SetProvinces(IEnumerable<IProvince> provinces)
    //{
    //    foreach (var province in provinces)
    //    {
    //        //var color = new Color(province.color.r, province.color.g, province.color.b);
    //        //foreach (var pos in province.block.elements)
    //        //{
    //        //    blockMap.SetCell(new Vector3Int(pos.x, pos.y), color);
    //        //}
    //    }

    //    var edges = Utilty.GenerateEdges(provinces.Select(x => x.block.edges));
    //    foreach (var pair in edges)
    //    {
    //        edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value, EdgeMap.EdgeType.Province);
    //    }

    //    mapUIContainer.SetProvinces(provinces);
    //}

    //internal void SetCountries(IEnumerable<ICountry> countries)
    //{
    //    foreach (var country in countries)
    //    {
    //        var color = new Color(country.color.r, country.color.g, country.color.b);
    //        foreach (var cellIndex in country.provinces.SelectMany(x => x.block.elements))
    //        {
    //            countryMap.SetCell(new Vector3Int(cellIndex.x, cellIndex.y), color);
    //        }
    //    }

    //    var countryEgdes = countries.Select(country =>
    //    {
    //        return country.provinces.SelectMany(prov => prov.block.edges)
    //            .Where(egde => Hexagon.GetNeighbors(egde).Any(n => country.provinces.All(x => !x.block.elements.Contains(n))));

    //    });

    //    var edges = Utilty.GenerateEdges(countryEgdes);
    //    foreach (var pair in edges)
    //    {
    //        edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value, EdgeMap.EdgeType.Country);
    //    }

    //    mapUIContainer.SetCountries(countries);
    //}
}
