using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    static class RiverBuilder
    {
        internal static Dictionary<(int x, int y), int> Build(Block[] blocks, Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        {
            var edges = Utilty.GenerateEdges(blocks.Select(x=>x.edges));

            var terrainsScales = terrains.ToDictionary(x => Hexagon.ScaleOffset(x.Key, 2), y => y.Value);

            var dictEdgeHeight = GenerateEdge2Height(terrainsScales, edges, random);

            var lineHeightOrders = dictEdgeHeight.OrderBy(_=>random.getNum(0, int.MaxValue)).OrderByDescending(x => x.Value).Select(x => x.Key);

            var majorRivers = GenerateMajorRiver(lineHeightOrders, dictEdgeHeight, terrainsScales);
            var minorRivers = GenerateMinorRiver(lineHeightOrders, dictEdgeHeight, terrainsScales, majorRivers);

            return majorRivers.Concat(minorRivers).SelectMany(x=>x).Distinct().ToDictionary(k => k, v=> edges[v]);
        }

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

        private static Dictionary<(int x, int y), int> GenerateEdge2Height(Dictionary<(int x, int y), TerrainType> terrainsScales, Dictionary<(int x, int y), int> edges, GRandom random)
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
                    if(!edges.ContainsKey(neighbor))
                    {
                        continue;
                    }
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
