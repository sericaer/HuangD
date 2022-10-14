using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    internal class BiomesMapBuilder
    {
        enum WetLevel
        {
            [Range(80, 100)]
            LEVEL1,
            [Range(60, 80)]
            LEVEL2,
            [Range(20, 60)]
            LEVEL3,
            [Range(0, 20)]
            LEVEL4
        }

        class RangeAttribute : Attribute
        {
            public int min;
            public int max;

            public RangeAttribute(int min, int max)
            {
                this.max = max;
                this.min = min;
            }
        }

        internal static Dictionary<(int x, int y), BiomeType> Build(Dictionary<(int x, int y), float> wetnessMap, 
            Dictionary<(int x, int y), TerrainType> terrains, 
            Dictionary<(int x, int y), float> heightMap,
            GRandom random)
        {
            var orderArray = wetnessMap.Where(x => terrains[x.Key] != TerrainType.Water)
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .ToArray();

            var position2WetnessOrder = new Dictionary<(int x, int y), int>();
            for (int i = 0; i < orderArray.Length; i++)
            {
                position2WetnessOrder.Add(orderArray[i], i);
            }

            var position2WetLevel = new Dictionary<(int x, int y), WetLevel>();

            foreach (var pair in position2WetnessOrder)
            {
                var percent = pair.Value * 100 / position2WetnessOrder.Count();

                foreach (WetLevel level in Enum.GetValues(typeof(WetLevel)))
                {
                    var range = level.GetAttribute<RangeAttribute>();
                    if (percent <= range.max && percent >= range.min)
                    {
                        position2WetLevel.Add(pair.Key, level);
                        break;
                    }
                }
            }

            var rslt = new Dictionary<(int x, int y), BiomeType>();

            var level4Positions = position2WetLevel.Where(x => x.Value == WetLevel.LEVEL4)
                .Select(x => x.Key)
                .OrderBy(x => heightMap[x])
                .ToArray();

            foreach (var position in level4Positions.Take(level4Positions.Count() / 10))
            {
                if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Marsh_Plain);
                }
            }

            foreach (var position in level4Positions.Except(rslt.Keys))
            {
                if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Juggle_Plain);
                }
                else if (terrains[position] == TerrainType.Hill)
                {
                    rslt.Add(position, BiomeType.Juggle_Hill);
                }
                else if (terrains[position] == TerrainType.Mount)
                {
                    rslt.Add(position, BiomeType.Grass_Mount);
                }
                else
                {
                    throw new Exception();
                }
            }

            foreach (var position in position2WetLevel.Where(x => x.Value == WetLevel.LEVEL3).Select(x => x.Key))
            {
                if (terrains[position] == TerrainType.Hill)
                {
                    rslt.Add(position, BiomeType.Forest_Hill);
                }
                else if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Forest_Plain);
                }

                else if (terrains[position] == TerrainType.Mount)
                {
                    rslt.Add(position, BiomeType.Grass_Mount);
                }
                else
                {
                    throw new Exception();
                }
            }

            foreach (var position in position2WetLevel.Where(x => x.Value == WetLevel.LEVEL2).Select(x => x.Key))
            {
                if (terrains[position] == TerrainType.Hill)
                {
                    rslt.Add(position, BiomeType.Grass_Hill);
                }
                else if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Grass_Plain);
                }
                else if (terrains[position] == TerrainType.Mount)
                {
                    rslt.Add(position, BiomeType.Grass_Mount);
                }
                else
                {
                    throw new Exception();
                }
            }

            foreach (var position in position2WetLevel.Where(x => x.Value == WetLevel.LEVEL1).Select(x => x.Key))
            {
                if (terrains[position] == TerrainType.Hill)
                {
                    rslt.Add(position, BiomeType.Desert_Hill);
                }
                else if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Desert_Plain);
                }
                else if (terrains[position] == TerrainType.Mount)
                {
                    rslt.Add(position, BiomeType.Desert_Mount);
                }
                else
                {
                    throw new Exception();
                }
            }

            BuildFarmPlain(ref rslt, random);
            BuildFarmHill(ref rslt, random);

            return rslt;
        }

        private static void BuildFarmHill(ref Dictionary<(int x, int y), BiomeType> rslt, GRandom random)
        {
            var forestPositions = rslt.Where(x => x.Value == BiomeType.Forest_Hill)
                            .OrderBy(_ => random.getNum(0, int.MaxValue))
                            .Select(x => x.Key)
                            .ToList();

            var rawforestPlainCount = forestPositions.Count();

            var queue = new UniqueQueue<(int x, int y)>();

            var start = forestPositions.First();
            queue.Enqueue(start);
            rslt[start] = BiomeType.Farm_Hill;

            forestPositions.Remove(start);

            while (forestPositions.Count > rawforestPlainCount * 0.9)
            {
                var curr = queue.Dequeue();

                var neighbors = Hexagon.GetNeighbors(curr)
                    .Where(x => forestPositions.Contains(x))
                    .ToArray();

                foreach (var neighbor in neighbors)
                {
                    if(random.isTrue(10))
                    {
                        rslt[neighbor] = BiomeType.Farm_Hill;

                        forestPositions.Remove(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }

                var newStarts = forestPositions.Take(3);
                foreach (var newStart in newStarts)
                {
                    rslt[newStart] = BiomeType.Farm_Hill;
                    forestPositions.Remove(newStart);
                    queue.Enqueue(newStart);
                }
            }
        }

        private static void BuildFarmPlain(ref Dictionary<(int x, int y), BiomeType> rslt, GRandom random)
        {
            var forestPositions = rslt.Where(x => x.Value == BiomeType.Forest_Plain)
                .OrderBy(_ => random.getNum(0, int.MaxValue))
                .Select(x => x.Key)
                .ToList();

            var rawforestPlainCount = forestPositions.Count();

            var queue = new UniqueQueue<(int x, int y)>();

            var start = forestPositions.First();
            queue.Enqueue(start);
            rslt[start] = BiomeType.Farm_Plain;

            forestPositions.Remove(start);

            while (forestPositions.Count > rawforestPlainCount * 0.7)
            {
                var curr = queue.Dequeue();

                var neighbors = Hexagon.GetNeighbors(curr)
                    .Where(x => forestPositions.Contains(x))
                    .ToArray();

                foreach (var neighbor in neighbors)
                {
                    rslt[neighbor] = BiomeType.Farm_Plain;

                    forestPositions.Remove(neighbor);
                    queue.Enqueue(neighbor);
                }

                var newStarts = forestPositions.Take(3);
                foreach (var newStart in newStarts)
                {
                    rslt[newStart] = BiomeType.Farm_Plain;
                    forestPositions.Remove(newStart);
                    queue.Enqueue(newStart);
                }
            }
        }
    }

    public static class EnumExtention
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }
}