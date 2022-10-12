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
    }

    public partial class Map
    {
        public static class Builder
        {
            public static IMap Build(MapInit mapInit, string seed, System.Action<string> processInfo)
            {

                var random = new GRandom(seed);

                processInfo.Invoke("创建噪音图");
                var noiseMap = NoiseMapBuilder.Build(random, mapInit.high, mapInit.width);

                processInfo.Invoke("创建地块图");
                var blocks = BlockBuilder.Build(noiseMap, random).ToArray();

                processInfo.Invoke("创建高程图");
                var block2Terrain = GroupByTerrainType(blocks, mapInit, random);
                var heightMap = HeightMapBuilder.Build(block2Terrain, noiseMap, random);

                processInfo.Invoke("创建地形图");
                var terrains = TerrainMapBuilder.Build(heightMap);

                processInfo.Invoke("创建降水图");
                var rainMap = RainMapBuilder.Build(terrains, random);

                processInfo.Invoke("创建河流图");
                var rivers = RiverBuilder.Build(heightMap, terrains, rainMap);
                var riverBanks = GenerateRiverBanks(rivers, terrains);

                processInfo.Invoke("创建湿度图");
                var wetnessMap = WetnessMapBuilder.Build(riverBanks, rainMap, terrains);

                var map = new Map();

                map.nosieMap = noiseMap;
                map.heightMap = heightMap;
                map.blocks = block2Terrain;
                map.terrains = terrains;
                map.rainMap = rainMap;
                map.rivers = rivers;
                //map.riverBanks = riverBanks;
                map.wetnessMap = wetnessMap;

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

            internal static Dictionary<Block, TerrainType> GroupByTerrainType(Block[] blocks, MapInit mapInit, GRandom random)
            {
                Debug.Log("GroupByTerrainType Start");

                var dict = new Dictionary<Block, TerrainType>();

                foreach (var block in GenerateWaterBlock(blocks, mapInit, random))
                {
                    dict.Add(block, TerrainType.Water);
                }

                blocks = blocks.Except(dict.Keys).ToArray();
                
                foreach (var block in blocks.Where(x => x.edges.Any(r => r.y == 0)))
                {
                    dict.Add(block, TerrainType.Mount);
                }

                var queue = new Queue<Block>(blocks.Except(dict.Keys));
                while(queue.Count != 0)
                {
                    var block = queue.Dequeue();

                    var neighors = dict.Keys.Where(b => b.neighors.Contains(block)).ToArray();
                    if(neighors.Length == 0)
                    {
                        queue.Enqueue(block);
                        continue;
                    }

                    var terrainType = CalcTerrainType(neighors.Select(n=> dict[n]), random);
                    dict.Add(block, terrainType);
                }

                Debug.Log("GroupByTerrainType End");

                return dict;
            }

            private static HashSet<Block> GenerateWaterBlock(Block[] blocks, MapInit mapInit, GRandom random)
            {
                var origins = blocks.Where(x => x.edges.Any(r => r.y == mapInit.width - 1 || r.x == 0))
                    .OrderBy(_=> random.getNum(0, int.MaxValue));

                var hashSet = new HashSet<Block>(origins);

                while (hashSet.Sum(x=>x.elements.Count) <= blocks.Sum(x=>x.elements.Count)*0.3)
                {
                    var curr = hashSet.ElementAt(random.getNum(0, hashSet.Count));
                    var next = curr.neighors.FirstOrDefault(x => !hashSet.Contains(x));
                    if(next != null)
                    {
                        hashSet.Add(next);
                    }
                }

                return hashSet.ToHashSet();
            }

            private static TerrainType CalcTerrainType(IEnumerable<TerrainType> neighors, GRandom random)
            {
                var percentGroup = new Dictionary<TerrainType, int>()
                    {
                        {TerrainType.Mount, 0 },
                        {TerrainType.Hill, 0 },
                        {TerrainType.Plain, 0 },
                        {TerrainType.Water, 0 },
                    };

                foreach (var type in neighors.OrderBy(_=>random.getNum(0, int.MaxValue)))
                {

                    switch(type)
                    {
                        case TerrainType.Water:
                            percentGroup[TerrainType.Plain] += 10;
                            break;
                        case TerrainType.Plain:
                            percentGroup[TerrainType.Hill] += 10;
                            percentGroup[TerrainType.Plain] += 30;
                            break;
                        case TerrainType.Hill:
                            percentGroup[TerrainType.Hill] += 20;
                            percentGroup[TerrainType.Plain] += 20;
                            percentGroup[TerrainType.Mount] += 5;
                            break;
                        case TerrainType.Mount:
                            percentGroup[TerrainType.Hill] += 10;
                            percentGroup[TerrainType.Mount] += 30;
                            break;
                        default:
                            throw new System.Exception($"not support type:{type}");
                    }
                }

                return random.RandomInGroup<TerrainType>(percentGroup.Select(x => (x.Value, x.Key)));
            }
        }
    }
}
