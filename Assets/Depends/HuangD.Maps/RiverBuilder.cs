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
        internal static Dictionary<(int x, int y), int> Build(Dictionary<(int x, int y), float> heightMap,
                                                      Dictionary<(int x, int y), TerrainType> terrains,
                                                      Dictionary<(int x, int y), float> rainMap)
        {
            var toWaterDistanceMap = GenerateToWaterDistanceMap(terrains);

            List<HashSet<(int x, int y)>> allRivers = GenerateRivers(heightMap, toWaterDistanceMap, rainMap);

            var rslt = new Dictionary<(int x, int y), int>();

            var startIndexs = allRivers.Select(x => x.First())
                .Select(x => Hexagon.ScaleOffset(x, 2))
                .Select(x => Hexagon.GetNeighbors(x).First());

            var baseMap = allRivers.SelectMany(x=>x)
                .Select(x => Hexagon.ScaleOffset(x, 2))
                .SelectMany(x => Hexagon.GetNeighbors(x))
                .Distinct();

            var waterEdges = toWaterDistanceMap.Where(p => p.Value == 0)
                .Select(p => Hexagon.ScaleOffset(p.Key,2))
                .SelectMany(x => Hexagon.GetNeighbors(x))
                .Distinct()
                .ToHashSet();

            Func<(int x, int y), bool> checkIsEnd = (p) =>
            {
                return rslt.ContainsKey(p) || waterEdges.Contains(p);
            };

            foreach (var start in startIndexs)
            {
                var path = Utilty.FindPath(start, checkIsEnd, baseMap);

                foreach (var index in path)
                {
                    rslt.TryAdd(index, 0);
                }
            }

            return rslt;
        }

        private static List<HashSet<(int x, int y)>> GenerateRivers(Dictionary<(int x, int y), float> heightMap, Dictionary<(int x, int y), int> toWaterDistanceMap, Dictionary<(int x, int y), float> rainMap)
        {

            var startIndexs = toWaterDistanceMap
                .OrderByDescending(x => System.Math.Pow(x.Value, 3) * System.Math.Pow(rainMap[x.Key], 4))
                .Select(x => x.Key)
                .ToList();

            var allRivers = new List<HashSet<(int x, int y)>>();

            while (allRivers.Sum(x => x.Count()) < heightMap.Count() / 10)
            {
                var start = startIndexs.First();
                startIndexs.Remove(start);

                var curr = start;
                var currValue = toWaterDistanceMap[start];

                var currRiverHashSet = new HashSet<(int x, int y)>();
                currRiverHashSet.Add(curr);

                bool isEndWithWater = true;

                while (true)
                {
                    var neighors = Hexagon.GetNeighbors(curr)
                        .Where(x => toWaterDistanceMap.ContainsKey(x) && toWaterDistanceMap[x] <= currValue)
                        .Where(x => Hexagon.GetRange(x, 1).Where(n => n != curr).All(n => !currRiverHashSet.Contains(n)));
                    if (neighors.Count() == 0)
                    {
                        isEndWithWater = false;
                        break;
                    }

                    var next = neighors.OrderBy(x => heightMap[x]).First();
                    currRiverHashSet.Add(next);

                    var intersectionsWithOtherRiver = Hexagon.GetRange(next, 1).Intersect(allRivers.SelectMany(x => x));
                    if (intersectionsWithOtherRiver.Any())
                    {
                        currRiverHashSet.Add(intersectionsWithOtherRiver.First());
                        break;
                    }

                    curr = next;
                    currValue = toWaterDistanceMap[curr];

                    if (currValue == 0)
                    {
                        break;
                    }
                }

                if (isEndWithWater)
                {
                    allRivers.Add(currRiverHashSet);
                    startIndexs = startIndexs.Except(currRiverHashSet.SelectMany(x => Hexagon.GetRange(x, 5)))
                       .ToList();
                }
            }

            return allRivers;
        }

        private static Dictionary<(int x, int y), int> GenerateToWaterDistanceMap(Dictionary<(int x, int y), TerrainType> terrains)
        {
            var coastLine  = terrains.Select(x => x.Key)
                                    .Where(x => terrains[x] == TerrainType.Water)
                                    .Where(x => Hexagon.GetNeighbors(x)
                                                       .Any(n => terrains.ContainsKey(n) && terrains[n] != TerrainType.Water));
            var map = coastLine.ToDictionary(k => k, _ => 0);

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
    }
}
