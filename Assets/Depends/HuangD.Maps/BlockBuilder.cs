using Math.TileMap;
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
    }
}
