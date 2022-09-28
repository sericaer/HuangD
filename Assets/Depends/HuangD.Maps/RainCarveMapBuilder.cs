using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public class RainCarve
    {
        public IEnumerable<(int x, int y)>[] trips;
        public Dictionary<(int x, int y), float> waterRushMap;
    }

    public class RainCarveMapBuilder
    {
        public static Dictionary<(int x, int y), float> heightMap;
        public static Dictionary<(int x, int y), TerrainType> terrains;
        public static Dictionary<(int x, int y), IEnumerable<(int x, int y)>> start2Trip;

        internal static RainCarve Build(Dictionary<(int x, int y), float> rainMap, Dictionary<(int x, int y), float> heightMap, Dictionary<(int x, int y), TerrainType> terrains)
        {
            RainCarveMapBuilder.heightMap = heightMap;
            RainCarveMapBuilder.terrains = terrains;
            RainCarveMapBuilder.start2Trip = new Dictionary<(int x, int y), IEnumerable<(int x, int y)>>();

            var waterRushMap = new Dictionary<(int x, int y), float>();

            foreach (var pair in terrains.Where(x=>x.Value != TerrainType.Water))
            {
                var start = pair.Key;
                var rainValue = rainMap[start];

                var trip = FindTrip(start);
                foreach(var pos in trip)
                {
                    if(!waterRushMap.ContainsKey(pos))
                    {
                        waterRushMap.Add(pos, rainValue);
                    }
                    else
                    {
                        waterRushMap[pos] += rainValue;
                    }
                    
                }
            }

            var rslt = new RainCarve();
            rslt.trips = start2Trip.Values.ToArray();
            rslt.waterRushMap = waterRushMap;

            return rslt;
        }

        private static IEnumerable<(int x, int y)> FindTrip((int x, int y) start)
        {
            var trip = new List<(int x, int y)>();

            var curr = start;
            while(true)
            {
                trip.Add(curr);

                if (terrains[curr] == TerrainType.Water)
                {
                    break;
                }

                if(start2Trip.ContainsKey(curr))
                {
                    trip.AddRange(start2Trip[curr]);
                    break;
                }

                var vaildNeighbors = Hexagon.GetNeighbors(curr)
                    .Where(x => heightMap.ContainsKey(x) && heightMap[x] < heightMap[curr])
                    .OrderBy(x => heightMap[x]);

                if(!vaildNeighbors.Any())
                {
                    break;
                }

                curr = vaildNeighbors.First();
            }

            if (!start2Trip.ContainsKey(start))
            {
                start2Trip.Add(start, trip);
            }

            return trip;
        }
    }
}