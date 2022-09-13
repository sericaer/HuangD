using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    static class RiverBuilder
    {
        internal static Dictionary<(int x, int y), int> Build(Dictionary<Block, TerrainType> block2Terrain, Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        {
            var dictScaleTerrain = terrains.ToDictionary(k => Hexagon.ScaleOffset(k.Key, 2), v => v.Value);

            var edges = Utilty.GenerateEdges(block2Terrain.Select(x => x.Key.edges));

            var dictEdgeHegiht = new Dictionary<(int x, int y), int>();
            foreach (var edge in edges.Keys)
            {
                var height = Hexagon.GetNeighbors(edge).Sum(x =>
                {
                    if (!dictScaleTerrain.ContainsKey(x))
                    {
                        return 1000;
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

            var rivers = new List<(int x, int y)>();

            var heigihOrders = dictEdgeHegiht.OrderBy(_=>random.getNum(0, int.MaxValue)).OrderByDescending(x => x.Value).Select(x => x.Key);

            while (rivers.Count() < 100 )
            {
                var startPoint = heigihOrders.First(x => !rivers.Contains(x));
                var currRivers = new List<(int x, int y)>() { startPoint };
                while (true)
                {
                    var vailds = currRivers.SelectMany(x => Hexagon.GetNeighbors(x).Where(n => !rivers.Concat(currRivers).Contains(n) && edges.ContainsKey(n))).OrderBy(x => dictEdgeHegiht[x]).ToArray();
                    if (vailds.Length == 0)
                    {
                        break;
                    }
                    if (Hexagon.GetNeighbors(vailds[0]).Any(x => dictScaleTerrain.ContainsKey(x) && dictScaleTerrain[x] == TerrainType.Water))
                    {
                        break;
                    }

                    currRivers.Add(vailds[0]);
                }
                rivers.AddRange(currRivers);
            }


            return rivers.ToDictionary(k => k, v => edges[v]);

            //var edges = Utilty.GenerateEdges(block2Terrain.Keys.Select(x => x.edges));

            //var land2WaterEdges = edges.Where(e =>
            //{
            //    var neighorTerrains = Hexagon.GetNeighbors(e.Key).Where(n => dictScaleTerrain.ContainsKey(n)).Select(n => dictScaleTerrain[n]);
            //    return neighorTerrains.Contains(TerrainType.Water) && neighorTerrains.Any(t => t != TerrainType.Water);
            //}).Select(x=>x.Key);

            //var landEdges = edges.Where(e => Hexagon.GetNeighbors(e.Key).All(n => dictScaleTerrain.ContainsKey(n) && dictScaleTerrain[n] != TerrainType.Water)).Select(x => x.Key);

            //var startPoint = land2WaterEdges.Where(x=>Hexagon.GetNeighbors(x).Any(n=> landEdges.Contains(n))).OrderBy(_ => random.getNum(0, int.MaxValue)).First();

            //var rivers = new List<(int x, int y)>() { startPoint };
            //while(true)
            //{
            //    var valids = Hexagon.GetNeighbors(startPoint).Where(x => landEdges.Contains(x) && !rivers.Contains(x)).ToArray();
            //    if(valids.Length == 0)
            //    {
            //        break;
            //    }

            //    rivers.Add(valids[0]);

            //    startPoint = valids[0];
            //}

            //return rivers.ToDictionary(k=>k, v=> edges[v]);

            //var waterEdges = block2Terrain.Where(x => x.Value == TerrainType.Water).Select(x => x.Key.edges);
            //var landEdges = block2Terrain.Where(x => x.Value != TerrainType.Water).Select(x => x.Key.edges);


            //foreach(var edge in waterEdges)
            //{
            //    var list = new List<(int x, int y)>();
            //}

            //var water2LandEdges = waterEdges.SelectMany(x => Hexagon.GetNeighbors(x).Where(n => landEdges.Contains(n)).Append(x));

            //waterEdges.Where
            //var edgesLand2Land = Utilty.GenerateEdges(block2Terrain.Where(x=>x.Value != TerrainType.Water).Select(x => x.Key.edges));
            //var edgesLand2Water = Utilty.GenerateEdges(block2Terrain.Where(x =>
            //{
            //    if (x.Value == TerrainType.Water)
            //    {
            //        return block2Terrain.Any(other => other.Value != TerrainType.Water && x.Key.isNeighbor(other.Key));
            //    }
            //    if (x.Value != TerrainType.Water)
            //    {
            //        return block2Terrain.Any(other => other.Value == TerrainType.Water && x.Key.isNeighbor(other.Key));
            //    }
            //    return false;

            //}).Select(x => x.Key.edges));

            //return edges.Where(e=>
            //{
            //    var neighorTerrains = Hexagon.GetNeighbors(e.Key).Where(n => dictScaleTerrain.ContainsKey(n)).Select(n => dictScaleTerrain[n]);
            //    return neighorTerrains.Contains(TerrainType.Water) && neighorTerrains.Any(t => t != TerrainType.Water);
            //}).ToDictionary(e=>e.Key, e=>e.Value);
        }
    }
}
