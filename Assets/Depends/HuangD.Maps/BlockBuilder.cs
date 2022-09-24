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
        internal static HashSet<Block> Build(Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {
            var groups = GroupPositions(noiseMap, random);
            return groups.Select(b => new Block(b)).ToHashSet();
        }

        private static HashSet<PositionGroup> GroupPositions(Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {
            var usedPositions = noiseMap.Keys.OrderBy(_ => random.getNum(0, int.MaxValue))
                    .Take(noiseMap.Count() / 50)
                    .ToHashSet();

            var block2Queue = usedPositions
                .ToDictionary(k => new PositionGroup(),
                              v => new Dictionary<(int x, int y), (float curr, float need)>() { { v, (noiseMap[v], noiseMap[v]) } });

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
                        if (usedPositions.Contains(next))
                        {
                            continue;
                        }

                        pos2Fill.Add(next, (0f, noiseMap[next]));
                        usedPositions.Add(next);
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
