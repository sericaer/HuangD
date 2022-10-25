using HuangD.Entities;
using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Maps
{
    public class MapInit
    {
        public int width;
        public int high;

        public Dictionary<TerrainType, int> terrainPercents;
    }

    public partial class Map
    {
        public static class Builder
        {

            public static IMap Build(MapInit mapInit, string seed, System.Action<string> processInfo)
            {

                var random = new GRandom(seed);
                
                var positions = Enumerable.Range(0, mapInit.high)
                                         .SelectMany(x => Enumerable.Range(0, mapInit.width).Select(y => (x, y)));

                processInfo.Invoke("创建噪音图");
                var noiseMap = NoiseMapBuilder.Build(positions, random);

                processInfo.Invoke("创建地块图");
                var blockMap = BlockBuilder.Build2(noiseMap, random);

                processInfo.Invoke("创建高程图");
                var heightMap = HeightMapBuilder.Build(blockMap, noiseMap, random);

                processInfo.Invoke("创建地形图");
                var terrains = TerrainMapBuilder.Build(heightMap, mapInit.terrainPercents);

                processInfo.Invoke("创建降水图");
                var rainMap = RainMapBuilder.Build(terrains, random);

                processInfo.Invoke("创建河流图");
                var rivers = RiverBuilder.Build(heightMap, terrains, rainMap);
                var riverBanks = GenerateRiverBanks(rivers, terrains);

                processInfo.Invoke("创建湿度图");
                var wetnessMap = WetnessMapBuilder.Build(riverBanks, rainMap, terrains);

                processInfo.Invoke("创建植被图");
                var biomesMap = BiomesMapBuilder.Build(wetnessMap, terrains, heightMap, random);

                processInfo.Invoke("创建人口图");
                var populationMap = PopulationMapBuilder.Build(biomesMap, random);

                var cells = positions.Select(x =>
                {
                    var cell = new Cell();
                    cell.position = x;
                    cell.block = blockMap[x].blockId;
                    cell.isBlockEdge = blockMap[x].isEdge;

                    cell.height = heightMap[x];
                    cell.noise = noiseMap[x];
                    cell.terrain = terrains[x];
                    cell.rain = rainMap[x];
                    cell.wetness = wetnessMap[x];

                    if (cell.terrain != TerrainType.Water)
                    {
                        cell.landInfo = new LandInfo();

                        cell.landInfo.biome = biomesMap[x];
                        cell.landInfo.population = populationMap[x];
                    }

                    return cell;
                }).ToArray();

                var riverItems = rivers.Keys.Select(x =>
                {
                    var item = new RiverMap.Item();
                    item.position = x;
                    item.index = rivers[x];

                    return item;
                }).ToArray();

                var map = new Map();

                map.blockMap = new BlockMap(cells);
                map.riverMap = new RiverMap(riverItems);

                return map;
            }

            private static Dictionary<(int x, int y), HashSet<(int x, int y)>> GenerateRiverBanks(Dictionary<(int x, int y), int> rivers, Dictionary<(int x, int y), TerrainType> terrains)
            {
                var rslt = new Dictionary<(int x, int y), HashSet<(int x, int y)>>();

                foreach (var index in rivers.Keys)
                {
                    var neighbors = Hexagon.GetNeighbors(index)
                        .Where(n => !rivers.ContainsKey(n))
                        .Select(n => Hexagon.ScaleOffset(n, 0.5f))
                        .Where(n => terrains.ContainsKey(n));

                    foreach(var neighor in neighbors)
                    {
                        if(!rslt.ContainsKey(neighor))
                        {
                            rslt.Add(neighor, new HashSet<(int x, int y)>());
                        }

                        foreach(var peer in neighbors.Where(n => n != neighor))
                        {
                            rslt[neighor].Add(peer);
                        }
                    }
                }

                return rslt;
            }
        }
    }
}
