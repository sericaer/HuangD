using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            var maxPlainIndex = orderPairs.Length / 10 * 8;
                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex), ref rslt);
                        }
                        break;
                    case TerrainType.Hill:
                        {
                            var maxPlainIndex = orderPairs.Length / 10;
                            var maxHillIndex = orderPairs.Length / 10 * 8;

                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
                            GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
                        }
                        break;
                    case TerrainType.Mount:
                        {
                            var maxPlainIndex = orderPairs.Length / 10;
                            var maxHillIndex = orderPairs.Length / 10 * 4;

                            GerneratePlainHeightDict(orderPairs.Take(maxPlainIndex), ref rslt);
                            GernerateHillHeightDict(orderPairs.Skip(maxPlainIndex).Take(maxHillIndex - maxPlainIndex), ref rslt);
                            GernerateMountHeightDict(orderPairs.Skip(maxHillIndex), ref rslt);
                        }
                        break;
                }
            }

            return rslt;
        }

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
    }
}
