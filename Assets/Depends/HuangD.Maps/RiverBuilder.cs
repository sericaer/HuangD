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

            var startIndexs = toWaterDistanceMap
                .OrderByDescending(x => System.Math.Pow(x.Value,3) * System.Math.Pow(rainMap[x.Key],4))
                .Select(x=>x.Key)
                .ToList();

            var allRivers = new List<HashSet<(int x, int y)>>();

            while(allRivers.Sum(x=>x.Count()) < terrains.Count()/10)
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

            var rslt = new Dictionary<(int x, int y), int>();
            foreach(var river in allRivers)
            {
                var baseMap = river.Select(x => Hexagon.ScaleOffset(x, 2))
                    .SelectMany(x => Hexagon.GetNeighbors(x))
                    .Distinct();

                var costMap = baseMap.ToDictionary(k => k, v =>
                  {
                      var average = Hexagon.GetNeighbors(v)
                      .Where(n => !baseMap.Contains(n))
                      .Select(n => Hexagon.ScaleOffset(n, 0.5f))
                      .Where(n=> heightMap.ContainsKey(n))
                      .Average(n => heightMap[n]);

                      return System.Math.Max(average, 0);
                  });

                var path = Utilty.FindPath(baseMap.First(), (p) => p == baseMap.Last(), baseMap, costMap);

                foreach (var index in path)
                {
                    rslt.TryAdd(index, 0);
                }

                rslt.TryAdd(baseMap.First(), 0);
                rslt.TryAdd(baseMap.Last(), 0);

            }

            return rslt;
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
