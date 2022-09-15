using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    static class RiverBuilder
    {
        internal static Dictionary<(int x, int y), int> Build(Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        {
            var terrainsScales = terrains.ToDictionary(x => Hexagon.ScaleOffset(x.Key, 2), y => y.Value);

            var dictEdgeHeight = GenerateEdge2Height(terrainsScales, random);

            var lineHeightOrders = dictEdgeHeight.OrderBy(_=>random.getNum(0, int.MaxValue)).OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            var rivers = new List<IEnumerable<(int x, int y)>>();
            while (rivers.Count < 15)
            {
                var select = lineHeightOrders.First();

                var path = Utilty.FindPath(select,
                        (pos) => Hexagon.GetNeighbors(pos).Any(x => terrainsScales.ContainsKey(x) && terrainsScales[x] == TerrainType.Water),
                        dictEdgeHeight.Keys.Where(x => !rivers.Any(r => r.Contains(x))).ToArray(),
                        dictEdgeHeight);
                if (path == null)
                {
                    lineHeightOrders.Remove(select);
                    continue;
                }

                rivers.Add(path);

                lineHeightOrders = lineHeightOrders.Except(path.SelectMany(x=>Hexagon.GetRange(x,5))).ToList();

                UpdateEdge2Height(dictEdgeHeight, path);
            }


            return rivers.SelectMany(x=>x).Distinct().ToDictionary(k => k, v=>0);
        }

        private static void UpdateEdge2Height(Dictionary<(int x, int y), int> dictEdgeHeight, IEnumerable<(int x, int y)> path)
        {
            foreach (var pos in path)
            {
                foreach(var elem in Hexagon.GetRange(pos, 5))
                {
                    if(dictEdgeHeight.ContainsKey(elem))
                    {
                        dictEdgeHeight[elem] = 100;
                    }
                }
            }
        }

        private static Dictionary<(int x, int y), int> GenerateEdge2Height(Dictionary<(int x, int y), TerrainType> terrainsScales, GRandom random)
        {
            var dictEdgeHeight = new Dictionary<(int x, int y), int>();

            foreach(var pair in terrainsScales)
            {
                int value = 0;
                switch (pair.Value)
                {
                    case TerrainType.Plain:
                        value = 1;
                        break;
                    case TerrainType.Hill:
                        value = 10;
                        break;
                    case TerrainType.Mount:
                        value = 50;
                        break;
                    case TerrainType.Water:
                        value = 0;
                        break;
                    default:
                        throw new System.Exception();
                }

                foreach (var neighbor in Hexagon.GetNeighbors(pair.Key))
                {
                    if(!dictEdgeHeight.ContainsKey(neighbor))
                    {
                        dictEdgeHeight.Add(neighbor, value);
                    }
                    else
                    {
                        dictEdgeHeight[neighbor] = (dictEdgeHeight[neighbor] + value) / 2;
                    }
                    
                }
            }

            return dictEdgeHeight;
        }
    }
}
