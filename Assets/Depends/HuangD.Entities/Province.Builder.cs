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
        private static IProvinceNameDef def;
        private static GRandom random;

        public static class Builder
        {
            public static IEnumerable<IProvince> Build(IMap map, string seed, IProvinceNameDef def)
            {
                Province.def = def;
                random = new GRandom(seed);

                var groups = GenerateProvinceBlocks(map);

                var randomNames = Province.def.names.OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

                Debug.Log($"randomNames{randomNames.Length}, groups{groups.Count}");

                var list = new List<IProvince>();
                for (int i = 0; i < groups.Count; i++)
                {
                    list.Add(new Province(randomNames[i], groups[i].cells));
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

                while(orderCells.Any())
                {
NewStart:
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
                        var curr = edges.OrderByDescending(cell=> map.blockMap[cell.position].landInfo.population).ElementAt(0);
                        edges.Remove(curr);

                        var neighors = Hexagon.GetNeighbors(curr.position)
                            .Where(p=> map.blockMap[p] != null && map.blockMap[p].landInfo != null);

                        foreach(var other in neighors.Where(p => !vaildCells.ContainsKey(p)))
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

                            if (group.cells.Count > 90 || (group.cells.Count > 20 &&group.cells.Sum(x=>x.landInfo.population) > 10*10000))
                            {
                                goto NewStart;
                            }
                        }
                    }
                }

                var smallGroups = rslt.Where(x => x.cells.Count <= 20).ToArray();
                rslt.RemoveAll(x => smallGroups.Contains(x));

                foreach(var group in smallGroups)
                {
                    var mergeTo = group.neighbors.Where(x=> !smallGroups.Contains(x))
                        .OrderBy(x => x.cells.Count)
                        .First();
                    mergeTo.cells.AddRange(group.cells);
                }

                return rslt;
            }

        }
    }
}
