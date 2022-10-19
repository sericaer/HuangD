using HuangD.Interfaces;
using HuangD.Maps;
using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    //public Camera mapCamera;
    //public Grid mapGrid;

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

    //public MapCanvas mapUIContainer;

    //private IMap mapData;

    // Start is called before the first frame update
    //void Start()
    //{
    //    MoveCameraToMapCenter();
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        var worldPoint = mapCamera.ScreenToWorldPoint(Input.mousePosition);
    //        var cellIndex = mapGrid.WorldToCell(worldPoint);
    //        var pos = (cellIndex.x, cellIndex.y);
    //        Debug.Log($"POS:{pos}, Height:{mapData.heightMap[pos]}, terrain:{mapData.terrains[pos]}, rain:{mapData.rainMap[pos]}, wetness:{mapData.wetnessMap[pos]}, biomes:{mapData.biomesMap[pos]}£¬ population{mapData.populationMap[pos]}");
    //    }
    //}

    //public void OnMove(Vector3 pos)
    //{
    //    Vector3 move = CaclMoveOffset(pos);

    //    mapCamera.transform.position = mapCamera.transform.position + move;
    //    //mapUIContainer.UpdateItemsPosition();
    //}

    //public void ScrollWheel(bool flag)
    //{
    //    mapCamera.orthographicSize = CalcNextScale(flag);
    //    //var alpha = (mapCamera.orthographicSize - 6) / (20 - 6);
    //    //foreach (var item in mapUIContainer.allCountryItem)
    //    //{
    //    //    item.SetAlpha(alpha);
    //    //    countryMap.SetAlpha(alpha);
    //    //}

    //    //foreach (var item in mapUIContainer.allProvinceItem)
    //    //{
    //    //    item.SetAlpha(1-alpha);
    //    //}

    //    //mapUIContainer.UpdateItemsPosition();
    //}

    internal void SetData(IMap map)
    {
        //mapData = map;

        //foreach (var pair in map.nosieMap)
        //{
        //    noiseMap.SetCell(pair.Key, pair.Value);
        //}
        //foreach (var pair in map.heightMap)
        //{
        //    heightMap.SetCell(pair.Key, pair.Value);
        //}
        //foreach (var pair in map.blocks)
        //{
        //    blockMap.SetBlock(pair.Key);
        //}
        //foreach (var pair in map.terrains)
        //{
        //    terrainMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        //}
        //foreach (var pos in map.rivers.Keys)
        //{
        //    riverMap.SetCell(new Vector3Int(pos.x, pos.y), map.rivers[pos]);
        //}
        //foreach(var pos in map.rainMap.Keys)
        //{
        //    rainMap.SetCell(new Vector3Int(pos.x, pos.y), map.rainMap[pos]);
        //}
        //foreach (var pos in map.wetnessMap.Keys)
        //{
        //    wetnessMap.SetCell(new Vector3Int(pos.x, pos.y), map.wetnessMap[pos]);
        //}
        //foreach (var pos in map.biomesMap.Keys)
        //{
        //    biomesMap.SetCell(new Vector3Int(pos.x, pos.y), map.biomesMap[pos]);
        //}
        //foreach (var pos in map.populationMap.Keys)
        //{
        //    populationMap.SetCell(new Vector3Int(pos.x, pos.y), map.populationMap[pos]);
        //}

        foreach(var cell in map.blockMap)
        {
            noiseMap.SetCell(cell.position, cell.noise);
            blockMap.SetCell(cell.position, cell.block);
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
