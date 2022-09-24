using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Maps
{
    static class TerrainBuilder
    {
        private static GRandom random { get; set; }

        //public static Dictionary<(int x, int y), TerrainType>  Build(IEnumerable<Block> blocks, int mapSize, string seed)
        //{
        //    random = new Maths.GRandom(seed);

        //    var rslt = new Dictionary<(int x, int y), TerrainType>();

        //    var terrainBlocks = GroupByTerrainType(blocks, mapSize);

        //    foreach (var dict in terrainBlocks.Select(x => GenrateTerrain(x.Key, x.Value)))
        //    {
        //        foreach (var pair in dict)
        //        {
        //            rslt.Add(pair.Key, pair.Value);
        //        }
        //    }

        //    return rslt;
        //}

        internal static Dictionary<(int x, int y), TerrainType> Build(Dictionary<Block, TerrainType> block2Terrain, Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {
            TerrainBuilder.random = random;

            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var lookup = block2Terrain.ToLookup(x => x.Value);
            foreach (TerrainType terrainType in Enum.GetValues(typeof(TerrainType)))
            {
                if(!lookup.Contains(terrainType))
                {
                    continue;
                }

                foreach(var pair in GenrateTerrain(terrainType, lookup[terrainType].Select(x=>x.Key), noiseMap))
                {
                    rslt.Add(pair.Key, pair.Value);
                }
            }
           
            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateTerrain(TerrainType terrainType, IEnumerable<Block> blocks, Dictionary<(int x, int y), float> noiseMap)
        {
            switch (terrainType)
            {
                case TerrainType.Mount:
                    return GenrateMount(blocks, noiseMap);
                case TerrainType.Hill:
                    return GenrateHill(blocks, noiseMap);
                case TerrainType.Plain:
                    return GenratePlain(blocks, noiseMap);
                case TerrainType.Water:
                    return GenrateWater(blocks, noiseMap);
                default:
                    throw new System.Exception();
            }
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateWater(IEnumerable<Block> blocks, Dictionary<(int x, int y), float> noiseMap)
        {
            var rslt = new Dictionary<(int x, int y), TerrainType>();
            var c = blocks.ToArray();
            var a = blocks.Distinct().ToArray();

            foreach (var elem in blocks.SelectMany(x => x.elements))
            {
                rslt.Add(elem, TerrainType.Water);
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenratePlain(IEnumerable<Block> blocks, Dictionary<(int x, int y), float> noiseMap)
        {
            //return blocks.SelectMany(x => x.elements).ToDictionary(x => x, _ => TerrainType.Plain);

            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            var orders = elements.OrderBy(x => noiseMap[x]).ToArray();

            for(int i=0; i<orders.Length; i++)
            {
                if(i< orders.Length * 0.8)
                {
                    rslt.Add(orders[i], TerrainType.Plain);
                }
                else
                {
                    rslt.Add(orders[i], TerrainType.Hill);
                }
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateHill(IEnumerable<Block> blocks, Dictionary<(int x, int y), float> noiseMap)
        {
            //return blocks.SelectMany(x => x.elements).ToDictionary(x => x, _ => TerrainType.Hill);

            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            var orders = elements.OrderBy(x => noiseMap[x]).ToArray();

            for (int i = 0; i < orders.Length; i++)
            {
                if (i < orders.Length * 0.1)
                {
                    rslt.Add(orders[i], TerrainType.Plain);
                }
                else if (i > orders.Length * 0.8)
                {
                    rslt.Add(orders[i], TerrainType.Mount);
                }
                else
                {
                    rslt.Add(orders[i], TerrainType.Hill);
                }
            }

            return rslt;

            //var rslt = new Dictionary<(int x, int y), TerrainType>();

            //var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            //var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            //while (elements.Count > rslt.Count * 10)
            //{
            //    var seeds = elements.Take(100).ToArray();
            //    foreach (var seed in seeds)
            //    {
            //        rslt.Add(seed, TerrainType.Plain);
            //        elements.Remove(seed);
            //    }

            //    foreach (var selected in rslt.Keys.ToArray())
            //    {
            //        var neighbors = Hexagon.GetNeighbors(selected).Where(x => !edges.Contains(x));
            //        var vailds = neighbors.Intersect(elements).ToArray();
            //        if (vailds.Length == 0)
            //        {
            //            continue;
            //        }

            //        var index = random.getNum(0, vailds.Length);
            //        rslt.Add(vailds[index], TerrainType.Plain);
            //        elements.Remove(vailds[index]);
            //    }
            //}

            //var mounts = new List<(int x, int y)>();
            //while (elements.Count > mounts.Count * 10)
            //{
            //    var seeds = elements.Take(10).ToArray();
            //    foreach (var seed in seeds)
            //    {
            //        mounts.Add(seed);
            //        elements.Remove(seed);
            //    }

            //    foreach (var selected in mounts.ToArray())
            //    {
            //        var neighbors = Hexagon.GetNeighbors(selected);
            //        var vailds = neighbors.Intersect(elements).ToArray();
            //        if (vailds.Length == 0)
            //        {
            //            continue;
            //        }

            //        var index = random.getNum(0, vailds.Length);
            //        mounts.Add(vailds[index]);
            //        elements.Remove(vailds[index]);
            //    }
            //}

            //foreach (var mount in mounts)
            //{
            //    rslt.Add(mount, TerrainType.Mount);
            //}

            //foreach (var elem in elements)
            //{
            //    rslt.Add(elem, TerrainType.Hill);
            //}

            //return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateMount(IEnumerable<Block> blocks, Dictionary<(int x, int y), float> noiseMap)
        {
            //return blocks.SelectMany(x=>x.elements).ToDictionary(x => x, _=>TerrainType.Mount);

            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            var orders = elements.OrderBy(x => noiseMap[x]).ToArray();

            for (int i = 0; i < orders.Length; i++)
            {
                if (i < orders.Length * 0.1)
                {
                    rslt.Add(orders[i], TerrainType.Plain);
                }
                else if (i > orders.Length * 0.4)
                {
                    rslt.Add(orders[i], TerrainType.Mount);
                }
                else
                {
                    rslt.Add(orders[i], TerrainType.Hill);
                }
            }

            return rslt;

            //var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            //var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            //var rslt = new Dictionary<(int x, int y), TerrainType>();
            //while (elements.Count > rslt.Count * 3)
            //{
            //    var seeds = elements.Take(20).ToArray();
            //    foreach (var seed in seeds)
            //    {
            //        rslt.Add(seed, TerrainType.Hill);
            //        elements.Remove(seed);
            //    }

            //    foreach (var selected in rslt.Keys.ToArray())
            //    {
            //        var neighbors = Hexagon.GetNeighbors(selected).Where(x => !edges.Contains(x));
            //        var vailds = neighbors.Intersect(elements).ToArray();
            //        if (vailds.Length == 0)
            //        {
            //            continue;
            //        }

            //        var index = random.getNum(0, vailds.Length);
            //        rslt.Add(vailds[index], TerrainType.Hill);
            //        elements.Remove(vailds[index]);
            //    }
            //}

            //var plains = new List<(int x, int y)>();
            //while (rslt.Count > plains.Count * 4)
            //{
            //    var seeds = rslt.Keys.Take(20).ToArray();
            //    foreach (var seed in seeds)
            //    {
            //        plains.Add(seed);
            //    }

            //    foreach (var selected in plains.ToArray())
            //    {
            //        var neighbors = Hexagon.GetNeighbors(selected);
            //        var vailds = neighbors.Intersect(rslt.Keys).ToArray();
            //        if (vailds.Length == 0)
            //        {
            //            continue;
            //        }

            //        var index = random.getNum(0, vailds.Length);
            //        plains.Add(vailds[index]);
            //    }
            //}

            //foreach (var plain in plains)
            //{
            //    rslt[plain] = TerrainType.Plain;
            //}

            //foreach (var elem in elements)
            //{
            //    rslt.Add(elem, TerrainType.Mount);
            //}

            //return rslt;
        }
    }
}
