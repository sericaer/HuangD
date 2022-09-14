using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    static class RiverBuilder
    {
        public class River
        {
            public LinkedList<Line> lines = new LinkedList<Line>();
            public class Line
            {
                public List<(int x, int y)> indexs;
                public bool isForward;

                public Line(List<(int x, int y)> indexs, bool isForward)
                {
                    this.indexs = indexs;
                    this.isForward = isForward;
                }

                internal (int x, int y) EndPoint()
                {
                    return isForward ? indexs.Last() : indexs.First();
                }
            }
        }

        internal static Dictionary<(int x, int y), int> Build(Dictionary<Block, TerrainType> block2Terrain, Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        {
            var terrainsScales = terrains.ToDictionary(x => Hexagon.ScaleOffset(x.Key, 2), y=>y.Value);

            var dictEdgeHeight = GenerateEdge2Height(terrainsScales);

            var lineHeightOrders = dictEdgeHeight.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            var select = lineHeightOrders.First();
            var river = new List<(int x, int y)>() { select };

            while(true)
            {
                var nexts = Hexagon.GetNeighbors(river.Last())
                    .Where(x => dictEdgeHeight.ContainsKey(x) && !river.Contains(x))
                    .Where(x => !Hexagon.GetNeighbors(x).Any(y=> y != river.Last() && river.Contains(y)))
                    .OrderBy(x => dictEdgeHeight[x]);

                if(nexts.Count() == 0)
                {
                    break;
                }

                select = nexts.First();
                river.Add(select);

                if(Hexagon.GetNeighbors(select).Any(x=> terrainsScales.ContainsKey(x) && terrainsScales[x] == TerrainType.Water))
                {
                    break;
                }
            }

            return river.ToDictionary(k => k, v=>0);
        }

        private static Dictionary<(int x, int y), int> GenerateEdge2Height(Dictionary<(int x, int y), TerrainType> terrainsScales)
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
                        value = 100;
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
                        dictEdgeHeight.Add(neighbor, 0);
                    }


                    dictEdgeHeight[neighbor] += value;
                }
            }

            return dictEdgeHeight;
        }



        //internal static Dictionary<(int x, int y), int> Build(Dictionary<Block, TerrainType> block2Terrain, Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        //{
        //    var edges = Utilty.GenerateEdges(block2Terrain.Select(x => x.Key.edges));

        //    var waterScales = terrains.Where(x => x.Value == TerrainType.Water).Select(x => Hexagon.ScaleOffset(x.Key, 2));

        //    var dictEdgeHegiht = GenerateEdge2Height(terrains, edges);

        //    var lines = GenerateEdgeLines(edges);

        //    var dictLineHegiht = lines.ToDictionary(k => k, v => v.Average(e => dictEdgeHegiht[e]));
        //    var lineHeightOrders = dictLineHegiht.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

        //    var rivers = new List<River>();

        //    while(rivers.SelectMany(x=>x.lines).SelectMany(x=>x.indexs).Count() < 100)
        //    {
        //        var highestLine = lineHeightOrders[0];
        //        lineHeightOrders.RemoveAt(0);

        //        var nextLines = GetIntersectLine(highestLine, dictLineHegiht.Keys).OrderBy(x => dictLineHegiht[x.indexs]);

        //        River river = new River();
        //        rivers.Add(river);

        //        river.lines.AddLast(nextLines.First());

        //        var selectLine = river.lines.Last.Value;

        //        while (true)
        //        {
        //            var intersectLines = GetNextIntersectLine(river.lines.Last.Value, dictLineHegiht.Keys)
        //                .Where(l => !rivers.SelectMany(x => x.lines).Any(al=>al.indexs == l.indexs));
        //            if (intersectLines.Count() == 0)
        //            {
        //                break;
        //            }

        //            selectLine = intersectLines
        //                .OrderBy(l => dictLineHegiht[l.indexs]).First();

        //            if (Hexagon.GetNeighbors(selectLine.EndPoint()).Any(n => waterScales.Contains(n)))
        //            {
        //                break;
        //            }

        //            river.lines.AddLast(selectLine);

        //            //if(selectLine.Any(p=> Hexagon.GetNeighbors(p).Any(n=> waterScales.Contains(n))))

        //        }
        //    }

        //    return rivers.SelectMany(x => x.lines).SelectMany(x => x.indexs).ToDictionary(k => k, v => edges[v]);
        //}

        private static IEnumerable<River.Line> GetIntersectLine(List<(int x, int y)> currLine, IEnumerable<List<(int x, int y)>> allLines)
        {
            var list = new List<River.Line>();
            foreach(var line in allLines)
            {
                if(Hexagon.GetNeighbors(currLine.First()).Any(n => n == line.First()))
                {
                    list.Add(new River.Line(line, true));
                }
                if (Hexagon.GetNeighbors(currLine.First()).Any(n => n == line.Last()))
                {
                    list.Add(new River.Line(line, false));
                }
            }

            return list;
        }

        private static IEnumerable<River.Line> GetNextIntersectLine(River.Line currLine, IEnumerable<List<(int x, int y)>> allLines)
        {
            var list = new List<River.Line>();
            foreach (var line in allLines)
            {
                if (Hexagon.GetNeighbors(currLine.EndPoint()).Any(n => n == line.First()))
                {
                    list.Add(new River.Line(line, true));
                }
                if (Hexagon.GetNeighbors(currLine.EndPoint()).Any(n => n == line.Last()))
                {
                    list.Add(new River.Line(line, false));
                }
            }

            return list;
        }

        private static List<List<(int x, int y)>> GenerateEdgeLines(Dictionary<(int x, int y), int> edges)
        {
            var joinInPoints = edges.Keys.Where(e => Hexagon.GetNeighbors(e).Count(n => edges.ContainsKey(n)) > 2).ToList();

            //return joinInPoints.ToDictionary(k => k, v => edges[v]);

            var usedJoinPoints = new HashSet<(int x, int y)>();
            var lines = new List<List<(int x, int y)>>();

            while (joinInPoints.Count() != usedJoinPoints.Count())
            {
                var line = new List<(int x, int y)>();
                lines.Add(line);

                var curr = joinInPoints.Except(usedJoinPoints).First();
                line.Add(curr);
                usedJoinPoints.Add(curr);

                while (true)
                {
                    var nextPoints = Hexagon.GetNeighbors(curr).Where(n => edges.Keys.Contains(n) && !lines.SelectMany(x => x).Contains(n)).ToArray();
                    if (nextPoints.Count() < 1)
                    {
                        break;
                    }

                    var notJoinPoints = nextPoints.Where(n => !joinInPoints.Contains(n)).ToArray();
                    if (notJoinPoints.Length == 1)
                    {
                        curr = notJoinPoints[0];
                        line.Add(curr);
                        continue;
                    }
                    else if (notJoinPoints.Length == 0)
                    {
                        if (nextPoints.Length == 1 && joinInPoints.Contains(nextPoints[0]))
                        {
                            line.Add(nextPoints[0]);
                            usedJoinPoints.Add(nextPoints[0]);
                        }

                        break;
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }

            return lines;
        }

        private static Dictionary<(int x, int y), int> GenerateEdge2Height(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<(int x, int y), int> edges)
        {
            var dictScaleTerrain = terrains.ToDictionary(k => Hexagon.ScaleOffset(k.Key, 2), v => v.Value);

            var dictEdgeHegiht = new Dictionary<(int x, int y), int>();
            foreach (var edge in edges.Keys)
            {
                var height = Hexagon.GetNeighbors(edge).Sum(x =>
                {
                    if (!dictScaleTerrain.ContainsKey(x))
                    {
                        return 0;
                    }

                    switch (dictScaleTerrain[x])
                    {
                        case TerrainType.Plain:
                            return 1;
                        case TerrainType.Hill:
                            return 10;
                        case TerrainType.Mount:
                            return 100;
                        default:
                            return 0;
                    }
                });

                dictEdgeHegiht.Add(edge, height);
            }

            return dictEdgeHegiht;
        }
    }
}
