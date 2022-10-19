using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HuangD.Maps
{
    public static class HeightMapBuilder
    {
        internal static Dictionary<(int x, int y), float> Build(Dictionary<Block, TerrainType> block2Terrain, Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {

            var rslt = new Dictionary<(int x, int y), float>();

            var lookup = block2Terrain.ToLookup(x => x.Value);
            foreach (var pair in lookup)
            {
                var terrainType = pair.Key;
                var blocks = pair.Select(x=>x.Key);

                var orderPairs = noiseMap.Where(x => blocks.Any(b=>b.elements.Contains(x.Key))).OrderBy(x => x.Value).ToArray();

                switch (terrainType)
                {
                    case TerrainType.Water:
                        GernerateWaterHeightDict(orderPairs, ref rslt);
                        break;
                    case TerrainType.Plain:
                        {
                            var maxPlainIndex = orderPairs.Length * 90 / 100;
                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex), ref rslt);
                        }
                        break;
                    case TerrainType.Hill:
                        {
                            var maxPlainIndex = orderPairs.Length * 30 / 100;
                            var maxHillIndex = orderPairs.Length  * 90 / 100;

                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
                            GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
                        }
                        break;
                    case TerrainType.Mount:
                        {
                            var maxPlainIndex = orderPairs.Length  * 10 / 100;
                            var maxHillIndex = orderPairs.Length   * 60 / 100;

                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
                            GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
                        }
                        break;
                }
            }

            var waterBlocks = block2Terrain.Keys.Where(k => block2Terrain[k] == TerrainType.Water);
            var nearByLandBlocks = waterBlocks.SelectMany(x => x.neighors).Where(n => block2Terrain[n] != TerrainType.Water);

            var coastLine = nearByLandBlocks.SelectMany(x => x.edges).Where(e => waterBlocks.Any(w => w.edges.Intersect(Hexagon.GetNeighbors(e)).Any()));
            foreach (var element in coastLine)
            {
                if(rslt[element] > 0.6)
                {
                    rslt[element] = 0.599f;
                }
                else if(rslt[element] > 0.3)
                {
                    rslt[element] = 0.299f;
                }
            }

            return rslt;
        }

        //internal static Dictionary<(int x, int y), float> Build(Dictionary<(int x, int y), int> blockMap, Dictionary<(int x, int y), float> noiseMap, GRandom random)
        //{

        //    var rslt = new Dictionary<(int x, int y), float>();

        //    var lookup = block2Terrain.ToLookup(x => x.Value);
        //    foreach (var pair in lookup)
        //    {
        //        var terrainType = pair.Key;
        //        var blocks = pair.Select(x => x.Key);

        //        var orderPairs = noiseMap.Where(x => blocks.Any(b => b.elements.Contains(x.Key))).OrderBy(x => x.Value).ToArray();

        //        switch (terrainType)
        //        {
        //            case TerrainType.Water:
        //                GernerateWaterHeightDict(orderPairs, ref rslt);
        //                break;
        //            case TerrainType.Plain:
        //                {
        //                    var maxPlainIndex = orderPairs.Length * 90 / 100;
        //                    GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
        //                    GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex), ref rslt);
        //                }
        //                break;
        //            case TerrainType.Hill:
        //                {
        //                    var maxPlainIndex = orderPairs.Length * 30 / 100;
        //                    var maxHillIndex = orderPairs.Length * 90 / 100;

        //                    GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
        //                    GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
        //                    GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
        //                }
        //                break;
        //            case TerrainType.Mount:
        //                {
        //                    var maxPlainIndex = orderPairs.Length * 10 / 100;
        //                    var maxHillIndex = orderPairs.Length * 60 / 100;

        //                    GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
        //                    GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
        //                    GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
        //                }
        //                break;
        //        }
        //    }

        //    var waterBlocks = block2Terrain.Keys.Where(k => block2Terrain[k] == TerrainType.Water);
        //    var nearByLandBlocks = waterBlocks.SelectMany(x => x.neighors).Where(n => block2Terrain[n] != TerrainType.Water);

        //    var coastLine = nearByLandBlocks.SelectMany(x => x.edges).Where(e => waterBlocks.Any(w => w.edges.Intersect(Hexagon.GetNeighbors(e)).Any()));
        //    foreach (var element in coastLine)
        //    {
        //        if (rslt[element] > 0.6)
        //        {
        //            rslt[element] = 0.599f;
        //        }
        //        else if (rslt[element] > 0.3)
        //        {
        //            rslt[element] = 0.299f;
        //        }
        //    }

        //    return rslt;
        //}

        private static void GernerateMountHeightDict(IEnumerable<KeyValuePair<(int x, int y), float>> orderPairs, ref Dictionary<(int x, int y), float> dict)
        {
            if(orderPairs.Count() == 0)
            {
                return;
            }

            var minValue = orderPairs.First().Value;
            var maxValue = orderPairs.Last().Value;

            foreach (var item in orderPairs)
            {
                dict.Add(item.Key, 0.6f + (item.Value - minValue) / (maxValue - minValue) * 0.4f);
            }
        }

        private static void GernerateHillHeightDict(IEnumerable<KeyValuePair<(int x, int y), float>> orderPairs, ref Dictionary<(int x, int y), float> dict)
        {
            if (orderPairs.Count() == 0)
            {
                return;
            }

            var minValue = orderPairs.First().Value;
            var maxValue = orderPairs.Last().Value;

            foreach (var item in orderPairs)
            {
                dict.Add(item.Key, 0.3f + (item.Value - minValue) / (maxValue - minValue) * 0.3f);
            }
        }

        private static void GerneratePlainHeightDict(IEnumerable<KeyValuePair<(int x, int y), float>> orderPairs, ref Dictionary<(int x, int y), float> dict)
        {
            if (orderPairs.Count() == 0)
            {
                return;
            }

            var minValue = orderPairs.First().Value;
            var maxValue = orderPairs.Last().Value;

            foreach (var item in orderPairs)
            {
                dict.Add(item.Key, (item.Value - minValue) / (maxValue - minValue) * 0.3f);
            }
        }

        private static void GernerateWaterHeightDict(IEnumerable<KeyValuePair<(int x, int y), float>> orderPairs, ref Dictionary<(int x, int y), float> dict)
        {
            if (orderPairs.Count() == 0)
            {
                return;
            }

            var minValue = orderPairs.First().Value;
            var maxValue = orderPairs.Last().Value;

            foreach(var item in orderPairs)
            {
                dict.Add(item.Key, - 1.0001f + (item.Value - minValue) / (maxValue - minValue));
            }
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
            while (queue.Count != 0)
            {
                var block = queue.Dequeue();

                var neighors = dict.Keys.Where(b => b.neighors.Contains(block)).ToArray();
                if (neighors.Length == 0)
                {
                    queue.Enqueue(block);
                    continue;
                }

                var terrainType = CalcTerrainType(neighors.Select(n => dict[n]), random);
                dict.Add(block, terrainType);
            }

            Debug.Log("GroupByTerrainType End");

            return dict;
        }

        private static HashSet<Block> GenerateWaterBlock(Block[] blocks, MapInit mapInit, GRandom random)
        {
            var origins = blocks.Where(x => x.edges.Any(r => r.y > mapInit.width - 6 || r.x < 5))
                .OrderBy(_ => random.getNum(0, int.MaxValue));

            var hashSet = new HashSet<Block>(origins);

            while (hashSet.Sum(x => x.elements.Count) <= blocks.Sum(x => x.elements.Count) * 0.3)
            {
                var curr = hashSet.ElementAt(random.getNum(0, hashSet.Count));
                var next = curr.neighors.FirstOrDefault(x => !hashSet.Contains(x));
                if (next != null)
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

            foreach (var type in neighors.OrderBy(_ => random.getNum(0, int.MaxValue)))
            {

                switch (type)
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
