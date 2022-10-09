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
