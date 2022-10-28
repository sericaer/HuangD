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
            [Range(70, 100)]
            LEVEL1,
            [Range(40, 70)]
            LEVEL2,
            [Range(20, 40)]
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

            BuildJuggleAndMarsh(terrains, heightMap, position2WetLevel, ref rslt);

            BuildForest(terrains, position2WetLevel, ref rslt);

            BuildGrass(terrains, position2WetLevel, ref rslt);

            BuildDesert(terrains, position2WetLevel, ref rslt);

            BuildFarmPlain(ref rslt, random);
            BuildFarmHill(ref rslt, random);

            return rslt;
        }

        private static void BuildGrass(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<(int x, int y), WetLevel> position2WetLevel, ref Dictionary<(int x, int y), BiomeType> rslt)
        {
            var level3Positions = position2WetLevel.Where(x => x.Value == WetLevel.LEVEL3).Select(x => x.Key).ToArray();
            foreach (var position in level3Positions)
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
        }

        private static void BuildForest(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<(int x, int y), WetLevel> position2WetLevel, ref Dictionary<(int x, int y), BiomeType> rslt)
        {
            var level2Positions = position2WetLevel.Where(x => x.Value == WetLevel.LEVEL2).Select(x => x.Key).ToArray();

            foreach (var position in level2Positions)
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

            var orderPositions = rslt.Where(x=>x.Value == BiomeType.Juggle_Hill || x.Value == BiomeType.Juggle_Plain)
                .Select(p => p.Key)
                .OrderByDescending(key => key.x)
                .ToArray();

            foreach (var position in orderPositions.Take(orderPositions.Count() / 5 ))
            {
                if (rslt[position] == BiomeType.Juggle_Hill)
                {
                    rslt[position] = BiomeType.Forest_Hill;
                }
                else if (rslt[position] == BiomeType.Juggle_Plain)
                {
                    rslt[position] = BiomeType.Forest_Plain;
                }
            }
        }

        private static void BuildJuggleAndMarsh(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<(int x, int y), float> heightMap, Dictionary<(int x, int y), WetLevel> position2WetLevel, ref Dictionary<(int x, int y), BiomeType> rslt)
        {
            var level1Positions = position2WetLevel.Where(x => x.Value == WetLevel.LEVEL1)
                .Select(x => x.Key)
                .OrderBy(x => heightMap[x])
                .ToArray();

            foreach (var position in level1Positions.Take(level1Positions.Count() / 10))
            {
                if (terrains[position] == TerrainType.Plain)
                {
                    rslt.Add(position, BiomeType.Marsh_Plain);
                }
            }

            foreach (var position in level1Positions.Except(rslt.Keys))
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
        }

        private static void BuildDesert(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<(int x, int y), WetLevel> position2WetLevel, ref Dictionary<(int x, int y), BiomeType> rslt)
        {
            var level4Positions = position2WetLevel.Where(x => x.Value == WetLevel.LEVEL4).Select(x => x.Key).ToArray();

            foreach (var position in level4Positions)
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
        }

        private static void BuildFarmHill(ref Dictionary<(int x, int y), BiomeType> rslt, GRandom random)
        {

            var forestPositions = rslt.Where(x => x.Value == BiomeType.Forest_Hill)
                            .OrderBy(_ => random.getNum(0, int.MaxValue))
                            .Select(p => p.Key)
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
            var maxX = rslt.Max(p => p.Key.x);
            var forestPositions = rslt.Where(x => x.Value == BiomeType.Forest_Plain || x.Value == BiomeType.Juggle_Plain)
                .Select(x => x.Key)
                .Where(k => k.x < maxX * 0.9)
                .OrderBy(_ => random.getNum(0, int.MaxValue))
                .ToList();

            var rawforestPlainCount = forestPositions.Count();

            var queue = new UniqueQueue<(int x, int y)>();

NewStart:
            var start = forestPositions.First();
            queue.Enqueue(start);
            rslt[start] = BiomeType.Farm_Plain;

            forestPositions.Remove(start);

            while (forestPositions.Count > rawforestPlainCount * 0.8)
            {
                if (queue.Count == 0)
                {
                    goto NewStart;
                }

                var curr = queue.Dequeue();

                var neighbors = Hexagon.GetNeighbors(curr)
                    .Where(x => forestPositions.Contains(x))
                    .ToArray();

                foreach (var neighbor in neighbors)
                {
                    if (rslt[neighbor] == BiomeType.Juggle_Plain)
                    {
                        if (random.isTrue(70))
                        {
                            continue;
                        }
                    }
                    if (rslt[neighbor] == BiomeType.Juggle_Plain)
                    {
                        if (random.isTrue(90))
                        {
                            continue;
                        }
                    }

                    rslt[neighbor] = BiomeType.Farm_Plain;

                    forestPositions.Remove(neighbor);
                    queue.Enqueue(neighbor);
                }

                if(forestPositions.Count % 10 == 0)
                {
                    goto NewStart;
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