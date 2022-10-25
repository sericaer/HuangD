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
                    list.Add(new Province(randomNames[i], groups[i]));
                }

                return list;
            }

            private static List<List<ICell>> GenerateProvinceBlocks(IMap map)
            {
                var landCells = map.blockMap.Where(x => x.landInfo != null).OrderBy(x => x.landInfo.population).ToList();

                var rslt = new List<List<ICell>>();

                while(landCells.Any())
                {
NewStart:
                    var start = landCells.First();
                    landCells.Remove(start);

                    var blocks = new List<ICell>() { start };
                    rslt.Add(blocks);

                    var edges = new List<ICell>();
                    edges.Add(start);

                    while (edges.Count != 0)
                    {
                        var curr = edges.ElementAt(random.getNum(0, edges.Count));
                        edges.Remove(curr);

                        foreach (var nextPosition in Hexagon.GetNeighbors(curr.position))
                        {
                            var next = landCells.FirstOrDefault(x => x.position == nextPosition);
                            if (next == null)
                            {
                                continue;
                            }

                            landCells.Remove(next);
                            blocks.Add(next);
                            edges.Add(next);

                            if(blocks.Count > 60 || blocks.Sum(x=>x.landInfo.population) > 10*10000)
                            {
                                goto NewStart;
                            }
                        }
                    }
                }

                return rslt;
            }

        }
    }
}
