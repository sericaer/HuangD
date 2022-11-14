using HuangD.Interfaces;
using HuangD.Mods;
using HuangD.Mods.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Entities
{
    public partial class Province
    {
        private static IProvinceDef def;
        private static GRandom random;

        public static class Builder
        {
            private const int maxCellCount = 30;
            private const int minCellCout = 9;
            private const int maxPopulation = 5 * 10000;
            private const int minPopulation = 5 * 1000;

            public static IEnumerable<IProvince> Build(IMap map, string seed, IProvinceDef def, IPopDef popDef)
            {
                Province.def = def;
                random = new GRandom(seed);

                var groups = GenerateProvinceBlocks(map);

                var randomNames = Province.def.names.OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

                Debug.Log($"randomNames{randomNames.Length}, groups{groups.Count}");

                var list = new List<IProvince>();
                for (int i = 0; i < groups.Count; i++)
                {
                    list.Add(new Province(randomNames[i], groups[i].cells, popDef));
                }

                for (int i = 0; i < groups.Count; i++)
                {
                    var province = list[i] as Province;
                    province.neighbors = groups[i].neighbors.Select(n=>
                    {
                        var index = groups.IndexOf(n);
                        return list[index];
                    }).ToHashSet();
                }

                return list;
            }

            public class CellGroup
            {
                public List<ICell> cells = new List<ICell>();
                public HashSet<CellGroup> neighbors = new HashSet<CellGroup>();
            }

            private static List<CellGroup> GenerateProvinceBlocks(IMap map)
            {
                var orderCells = map.blockMap.Where(x => x.landInfo != null).OrderBy(x => x.landInfo.population).ToList();
                var vaildCells = orderCells.ToDictionary(x => x.position, x => x);

                var pos2Group = new Dictionary<(int x, int y), CellGroup>();
                var rslt = new List<CellGroup>();

NewStart:
                while (orderCells.Any())
                {
                    var start = orderCells.First();
                    orderCells.Remove(start);
                    vaildCells.Remove(start.position);

                    var group = new CellGroup();
                    group.cells.Add(start);

                    rslt.Add(group);
                    pos2Group.Add(start.position, group);

                    var edges = new List<ICell>();
                    edges.Add(start);

                    while (edges.Count != 0)
                    {
                        var curr = edges.OrderBy(cell=> map.blockMap[cell.position].noise).ElementAt(0);
                        edges.Remove(curr);

                        var neighors = Hexagon.GetNeighbors(curr.position)
                            .Where(p=> map.blockMap[p] != null && map.blockMap[p].landInfo != null);

                        foreach(var other in neighors.Where(p => !vaildCells.ContainsKey(p) && !group.cells.Any(g=>g.position == p)))
                        {
                            group.neighbors.Add(pos2Group[other]);
                            pos2Group[other].neighbors.Add(group);
                        }

                        foreach (var nextPosition in neighors.Where(p=>vaildCells.ContainsKey(p))
                            .OrderByDescending(p=> vaildCells[p].landInfo.population))
                        {
                            var next = vaildCells[nextPosition];
                            orderCells.Remove(next);
                            vaildCells.Remove(nextPosition);

                            group.cells.Add(next);
                            edges.Add(next);
                            pos2Group.Add(next.position, group);

                            if (group.cells.Count > maxCellCount || (group.cells.Count > minCellCout &&group.cells.Sum(x=>x.landInfo.population) > maxPopulation))
                            {
                                goto NewStart;
                            }
                        }
                    }
                }

                var smallGroups = new Queue<CellGroup>(rslt.Where(x => x.neighbors.Count != 0)
                    .Where(x=> x.cells.Count <= minCellCout || x.cells.Sum(x=>x.landInfo.population) < minPopulation));
                rslt.RemoveAll(x => smallGroups.Contains(x));


                int noMergeCount = 0;

                while (smallGroups.Count != 0)
                {
                    var curr = smallGroups.Dequeue();

                    var mergeTo = curr.neighbors.Where(x=>rslt.Contains(x))
                        .OrderBy(x => x.cells.Sum(x=> x.landInfo.population)).FirstOrDefault();
                    if(mergeTo == null)
                    {
                        smallGroups.Enqueue(curr);
                        noMergeCount++;

                        if(noMergeCount == smallGroups.Count)
                        {
                            break;
                        }

                        continue;
                    }

                    noMergeCount = 0;

                    mergeTo.cells.AddRange(curr.cells);

                    foreach (var neighbor in curr.neighbors.Where(x => x != mergeTo))
                    {
                        mergeTo.neighbors.Add(neighbor);

                        neighbor.neighbors.Remove(curr);
                        neighbor.neighbors.Add(mergeTo);
                    }
                }

                foreach (var group in rslt)
                {
                    group.neighbors.IntersectWith(rslt);
                }

                return rslt;
            }

        }
    }
}
