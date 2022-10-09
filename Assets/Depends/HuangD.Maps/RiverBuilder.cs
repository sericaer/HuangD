using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Maps
{
    static class RiverBuilder
    {
        private static Dictionary<(int x, int y), float> heightMap;
        private static Dictionary<(int x, int y), TerrainType> terrains;
        private static Dictionary<(int x, int y), float> rainMap;
        private static GRandom random;

        //internal static Dictionary<(int x, int y), int> Build(IEnumerable<Block> blocks, Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        //{
        //    var edges = Utilty.GenerateEdges(blocks.Select(x=>x.edges));

        //    var terrainsScales = terrains.ToDictionary(x => Hexagon.ScaleOffset(x.Key, 2), y => y.Value);

        //    var dictEdgeHeight = GenerateEdge2Height(terrainsScales, random).ToDictionary(k => k.Key, v => edges.ContainsKey(v.Key) ? v.Value / 3 : v.Value);

        //    var lineHeightOrders = dictEdgeHeight.OrderBy(_=>random.getNum(0, int.MaxValue)).OrderByDescending(x => x.Value).Select(x => x.Key);

        //    var majorRivers = GenerateMajorRiver(lineHeightOrders, dictEdgeHeight, terrainsScales);
        //    var minorRivers = GenerateMinorRiver(lineHeightOrders, dictEdgeHeight, terrainsScales, majorRivers);

        //    return majorRivers.Concat(minorRivers).SelectMany(x=>x).Distinct().ToDictionary(k => k, v=> 0);
        //}

        //internal static Dictionary<(int x, int y), int> Build(RainCarve rainCarveMap)
        //{
        //    var selectRiverPoint = rainCarveMap.waterRushMap.OrderByDescending(x => x.Value)
        //        .Take(rainCarveMap.waterRushMap.Count() * 30 / 100)
        //        .Select(k => k.Key);

        //    return rainCarveMap.trips.OrderByDescending(x => selectRiverPoint.Intersect(x).Count())
        //        .Take(3)
        //        .SelectMany(x => x)
        //        .Distinct()
        //        .ToDictionary(k => k, _ => 0);
        //}

        private static IEnumerable<IEnumerable<(int x, int y)>> GenerateMinorRiver(IEnumerable<(int x, int y)> lineHeightOrders, Dictionary<(int x, int y), int> dictEdgeHeight, Dictionary<(int x, int y), TerrainType> terrainsScales, IEnumerable<IEnumerable<(int x, int y)>> majorRivers)
        {
            var rivers = new List<IEnumerable<(int x, int y)>>();

            var currLineHeightOrders = lineHeightOrders.ToList();
            var currDictEdgeHeight = dictEdgeHeight.ToDictionary(k => k.Key, v => v.Value);
            while (rivers.Count < 20)
            {
                var select = currLineHeightOrders.First();

                var path = Utilty.FindPath(select,
                        (pos) => Hexagon.GetNeighbors(pos).Any(x => terrainsScales.ContainsKey(x) && terrainsScales[x] == TerrainType.Water)
                               || majorRivers.Concat(rivers).SelectMany(x=>x).Contains(pos), 
                        currDictEdgeHeight.Keys.Where(x => !rivers.Any(r => r.Contains(x))).ToArray(),
                        currDictEdgeHeight);
                if (path == null)
                {
                    currLineHeightOrders.Remove(select);
                    continue;
                }

                currLineHeightOrders = currLineHeightOrders.Except(path.SelectMany(x => Hexagon.GetRange(x, 5))).ToList();

                if (path.Count() < 20)
                {
                    continue;
                }

                rivers.Add(path);

                UpdateEdge2Height(currDictEdgeHeight, path, 5, 50);
            }

            return rivers;
        }

        internal static Dictionary<(int x, int y), int> Build(Dictionary<(int x, int y), float> heightMap,
                                                      Dictionary<(int x, int y), TerrainType> terrains,
                                                      Dictionary<(int x, int y), float> rainMap)
        {

            var toWaterDistanceMap = GenerateToWaterDistanceMap(terrains);

            var allRivers = new List<(int x, int y)>();

            while(allRivers.Count() < terrains.Count()/10)
            {
                
                var start = toWaterDistanceMap.OrderByDescending(x => System.Math.Pow(x.Value,5) * System.Math.Pow(rainMap[x.Key],4)).First();
                toWaterDistanceMap.Remove(start.Key);

                if (Hexagon.GetNeighbors(start.Key).Intersect(allRivers).Any())
                {
                    toWaterDistanceMap.Remove(start.Key);
                    continue;
                }

                var curr = start.Key;
                var currValue = start.Value;

                var currRiverHashSet = new HashSet<(int x, int y)>();
                currRiverHashSet.Add(curr);

                while (true)
                {
                    var neighors = Hexagon.GetNeighbors(curr)
                        .Where(x => toWaterDistanceMap.ContainsKey(x) && toWaterDistanceMap[x] <= currValue)
                        .Where(x => Hexagon.GetRange(x, 1).Where(n => n != curr).All(n => !currRiverHashSet.Contains(n)));
                    if (neighors.Count() == 0)
                    {
                        break;
                    }

                    var next = neighors.OrderBy(x => heightMap[x]).First();
                    currRiverHashSet.Add(next);

                    if (Hexagon.GetNeighbors(next).Intersect(allRivers).Any())
                    {
                        toWaterDistanceMap.Remove(curr);
                        break;
                    }

                    curr = next;
                    currValue = toWaterDistanceMap[curr];

                    toWaterDistanceMap.Remove(curr);

                    if (currValue == 1)
                    {
                        break;
                    }
                }

                allRivers.AddRange(currRiverHashSet);
            }

            return allRivers.Distinct()
                //.Select(x=>Hexagon.ScaleOffset(x,2))
                .ToDictionary(x => x, _ => 0);
            //RiverBuilder.heightMap = heightMap;
            //RiverBuilder.terrains = terrains;
            //RiverBuilder.rainMap = rainMap;
            //RiverBuilder.random = random;

            //var origins = terrains.Where(x => x.Value == TerrainType.Mount)
            //    .Select(x => x.Key)
            //    //.Where(x => rainMap[x] > 0.4f)
            //    .OrderBy(x =>
            //    {
            //        switch (terrains[x])
            //        {
            //            case TerrainType.Hill:
            //                return 1;
            //            case TerrainType.Plain:
            //                return 0;
            //            case TerrainType.Mount:
            //                return 100;
            //            default:
            //                throw new Exception();
            //        }
            //    }).ToArray();

            //List<(int x, int y)> river = null;
            //for (int i = 0; i < origins.Length; i++)
            //{
            //    river = GenerateMajorRiver(origins[0]);
            //    if (river.Count > 50)
            //    {
            //        break;
            //    }
            //}

            //return river.ToDictionary(k => Hexagon.ScaleOffset(k, 2), v => 0);
        }

        private static Dictionary<(int x, int y), int> GenerateToWaterDistanceMap(Dictionary<(int x, int y), TerrainType> terrains)
        {
            var coastLine  = terrains.Select(x => x.Key)
                                    .Where(x => terrains[x] != TerrainType.Water)
                                    .Where(x => Hexagon.GetNeighbors(x)
                                                       .Any(n => terrains.ContainsKey(n) && terrains[n] == TerrainType.Water));
            var map = coastLine.ToDictionary(k => k, _ => 1);

            var queue = new Queue<(int x, int y)>(map.Keys);
            while(queue.Count !=0)
            {
                var curr = queue.Dequeue();

                foreach(var next in Hexagon.GetNeighbors(curr).Where(n => terrains.ContainsKey(n) && terrains[n] != TerrainType.Water))
                {
                    var nextValue = map[curr] + 1;
                    if(!map.ContainsKey(next) || map[next] > nextValue)
                    {
                        map[next] = nextValue;
                        queue.Enqueue(next);
                    }
                }
            }

            return map;
        }

        //internal static Dictionary<(int x, int y), int> Build(Dictionary<(int x, int y), float> heightMap, 
        //                                                      Dictionary<(int x, int y), TerrainType> terrains, 
        //                                                      Dictionary<(int x, int y), float> rainMap, 
        //                                                      GRandom random)
        //{
        //    RiverBuilder.heightMap = heightMap;
        //    RiverBuilder.terrains = terrains;
        //    RiverBuilder.rainMap = rainMap;
        //    RiverBuilder.random = random;

        //    var origins = terrains.Where(x => x.Value == TerrainType.Mount)
        //        .Select(x => x.Key)
        //        //.Where(x => rainMap[x] > 0.4f)
        //        .OrderBy(x => 
        //        {
        //            switch(terrains[x])
        //            {
        //                case TerrainType.Hill:
        //                    return 1;
        //                case TerrainType.Plain:
        //                    return 0;
        //                case TerrainType.Mount:
        //                    return 100;
        //                default:
        //                    throw new Exception();
        //            }
        //        }).ToArray();

        //    List<(int x, int y)> river = null;
        //    for (int i=0; i< origins.Length; i++)
        //    {
        //        river = GenerateMajorRiver(origins[0]);
        //        if(river.Count > 50)
        //        {
        //            break;
        //        }
        //    }

        //    return river.ToDictionary(k => Hexagon.ScaleOffset(k, 2), v => 0);
        //}

        private static List<(int x, int y)> GenerateMajorRiver((int x, int y) origin)
        {
            var path = Utilty.FindPath(origin,
                       (pos) => Hexagon.GetNeighbors(pos).Any(x=> terrains.ContainsKey(x) && terrains[x] == TerrainType.Water),
                       terrains.Keys,
                       heightMap.ToDictionary(k=>k.Key, v=>(int)(v.Value * 10000)));

            return path.ToList();
        }

        private static IEnumerable<IEnumerable<(int x, int y)>> GenerateMajorRiver(IEnumerable<(int x, int y)> lineHeightOrders, Dictionary<(int x, int y), int> dictEdgeHeight, Dictionary<(int x, int y), TerrainType> terrainsScales)
        {
            var mainRivers = new List<IEnumerable<(int x, int y)>>();

            var currLineHeightOrders = lineHeightOrders.ToList();
            var currDictEdgeHeight = dictEdgeHeight.ToDictionary(k => k.Key, v => v.Value);
            while (mainRivers.Count < 3)
            {
                var select = currLineHeightOrders.First();

                var path = Utilty.FindPath(select,
                        (pos) => Hexagon.GetNeighbors(pos).Any(x => terrainsScales.ContainsKey(x) && terrainsScales[x] == TerrainType.Water),
                        currDictEdgeHeight.Keys.Where(x => !mainRivers.Any(r => r.Contains(x))).ToArray(),
                        currDictEdgeHeight);
                if (path == null)
                {
                    currLineHeightOrders.Remove(select);
                    continue;
                }

                currLineHeightOrders = currLineHeightOrders.Except(path.SelectMany(x => Hexagon.GetRange(x, 5))).ToList();

                if(path.Count() < 100)
                {
                    continue;
                }

                mainRivers.Add(path);

                UpdateEdge2Height(currDictEdgeHeight, path, 10, 50);
            }

            return mainRivers;
        }

        private static void UpdateEdge2Height(Dictionary<(int x, int y), int> dictEdgeHeight, IEnumerable<(int x, int y)> path, int range, int value)
        {
            foreach (var pos in path)
            {
                foreach(var elem in Hexagon.GetRange(pos, range))
                {
                    if(dictEdgeHeight.ContainsKey(elem))
                    {
                        dictEdgeHeight[elem] = value;
                    }
                }
            }
        }

        private static Dictionary<(int x, int y), int> GenerateEdge2Height(Dictionary<(int x, int y), TerrainType> terrainsScales, GRandom random)
        {
            var dictEdgeHeight = new Dictionary<(int x, int y), int>();

            Func<TerrainType, int> funcGetHegiht = (terrain) =>
            {
                switch (terrain)
                {
                    case TerrainType.Plain:
                        return 10;
                    case TerrainType.Hill:
                        return 50;
                    case TerrainType.Mount:
                        return 100;
                    case TerrainType.Water:
                        return 0;
                    default:
                        throw new System.Exception();
                }
            };

            int maxX = terrainsScales.Keys.Max(k => k.x);
            int maxY = terrainsScales.Keys.Max(k => k.y);
            for(int x=0; x<maxX; x++)
            {
                for(int y=0; y<maxY; y++)
                {
                    var pos = (x, y);
                    if(terrainsScales.ContainsKey(pos))
                    {
                        continue;
                    }

                    var average = (int)Hexagon.GetNeighbors(pos)
                        .Where(p => terrainsScales.ContainsKey(p))
                        .Average(p => funcGetHegiht(terrainsScales[p]));

                    dictEdgeHeight.Add(pos, average);
                }
            }
            return dictEdgeHeight;
        }
    }
}
