﻿using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using PositionGroup = System.Collections.Generic.HashSet<(int x, int y)>;

namespace HuangD.Maps
{
    static class BlockBuilder
    {
        internal static Block[] Build(Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {

            Dictionary<PositionGroup, List< PositionGroup >> topoMap;
            var groups = GroupPositions(noiseMap, random, out topoMap);

            var blocks = groups.Select(b => new Block(b)).ToHashSet();
            foreach(var block in blocks)
            {
                block.neighors = topoMap[block.elements].Select(x => blocks.SingleOrDefault(y => y.elements == x)).ToHashSet();
            }

            return blocks.OrderBy(_=> random.getNum(0, int.MaxValue)).ToArray();
        }

        private static HashSet<PositionGroup> GroupPositions(Dictionary<(int x, int y), float> noiseMap, GRandom random, out Dictionary<PositionGroup, List<PositionGroup>> topoMap)
        {
            var origins = noiseMap.Keys.OrderBy(_ => random.getNum(0, int.MaxValue))
                    .Take(noiseMap.Count() / 50).ToArray();

            var pos2Group = origins.ToDictionary(k => k, v => new PositionGroup());

            var block2Queue = pos2Group.ToDictionary(k => k.Value,
                  v => new Dictionary<(int x, int y), (float curr, float need)>() { { v.Key, (noiseMap[v.Key], noiseMap[v.Key]) } });

            topoMap = block2Queue.ToDictionary(k => k.Key, _ => new List<PositionGroup>());

            while (block2Queue.Keys.Sum(x => x.Count) != noiseMap.Count)
            {
                foreach (var pair in block2Queue)
                {
                    var block = pair.Key;
                    var pos2Fill = pair.Value;

                    if (pos2Fill.Count == 0)
                    {
                        continue;
                    }

                    var pos2FillPair = pos2Fill.ElementAt(random.getNum(0, pos2Fill.Count()));

                    var pos = pos2FillPair.Key;
                    var fill = pos2FillPair.Value;

                    var currValue = fill.curr + 0.1f;
                    if (currValue < fill.need)
                    {
                        pos2Fill[pos] = (currValue, fill.need);
                        continue;
                    }

                    pos2Fill.Remove(pos);
                    block.Add(pos);

                    foreach (var next in Hexagon.GetNeighbors(pos).Where(n => noiseMap.ContainsKey(n)))
                    {
                        if (pos2Group.ContainsKey(next))
                        {
                            topoMap[block].Add(pos2Group[next]);
                            continue;
                        }

                        pos2Fill.Add(next, (0f, noiseMap[next]));
                        pos2Group.Add(next, block);
                    }
                }
            }

            var blocks = block2Queue.Keys;

            var dict = blocks.SelectMany(b => b).GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .ToDictionary(g => g.Key, g => g.Count());
            return blocks.ToHashSet();
        }

        internal static Dictionary<(int x, int y), int> Build2(Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {
            var origins = noiseMap.Keys.OrderBy(_ => random.getNum(0, int.MaxValue))
                    .Take(noiseMap.Count() / 50)
                    .ToArray();

            var blockGroups = new List<BlockGroup>(origins.Select(x=>new BlockGroup(x)));

            var vaildDict = noiseMap.Keys.Except(origins)
                .ToDictionary(k => k, v => noiseMap[v]);

            while (vaildDict.Count != 0)
            {
                foreach(var group in blockGroups)
                {
                    group.IncElement(vaildDict);
                }
            }

            var rslt = new Dictionary<(int x, int y), int>();
            for(int i=0; i<blockGroups.Count; i++)
            {
                foreach(var pos in blockGroups[i].all)
                {
                    rslt.Add(pos, i);
                }
            }

            return rslt;
        }

        class BlockGroup
        {
            public IEnumerable<(int x, int y)> all => _elements.Concat(edgeDict.Keys);

            public Dictionary<(int x, int y), ItemValue> edgeDict = new Dictionary<(int x, int y), ItemValue>();

            private List<(int x, int y)> _elements = new List<(int x, int y)>();

            public BlockGroup((int x, int y) star)
            {
                edgeDict.Add(star, new ItemValue(0.0f, 0.0f));
            }

            public void IncElement(Dictionary<(int x, int y), float> vaildDict)
            {
                foreach (var key in edgeDict.Keys.ToArray())
                {
                    edgeDict[key].curr += 0.1f;
                    if (!edgeDict[key].isFull)
                    {
                        continue;
                    }

                    edgeDict.Remove(key);
                    _elements.Add(key);

                    foreach (var next in Hexagon.GetNeighbors(key).Where(n => vaildDict.ContainsKey(n)).ToArray())
                    {
                        if(_elements.Contains(next) || edgeDict.ContainsKey(next))
                        {
                            continue;
                        }

                        edgeDict.Add(next, new ItemValue(0.0f, vaildDict[next]));
                        vaildDict.Remove(next);
                    }
                }
            }

            public class ItemValue
            {
                public float curr;
                public float need;

                public ItemValue(float curr, float need)
                {
                    this.curr = curr;
                    this.need = need;
                }

                public bool isFull => curr >= need;
            }
        }
    }
}
