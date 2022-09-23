using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using Block = System.Collections.Generic.HashSet<(int x, int y)>;

namespace HuangD.Maps
{
    static class BlockBuilder
    {
        internal static HashSet<Block> Build(IEnumerable<(int x, int y)> mapPositions, GRandom random)
        {
            var whiteNoiseMap = mapPositions
                        .ToDictionary(k => k, _ => random.getNum(0f, 1.0f));

            var cellularMap = whiteNoiseMap.ToDictionary(k => k.Key, v =>
            {
                var range = Hexagon.GetNeighbors(v.Key).Where(r => whiteNoiseMap.ContainsKey(r));
                var max = range.Max(b => whiteNoiseMap[b]);
                var min = range.Min(b => whiteNoiseMap[b]);
                return 1 - max < min ? max : min;
            });

            var usedPositions = cellularMap.Keys.OrderBy(_ => random.getNum(0, int.MaxValue))
                    .Take(cellularMap.Count() / 50)
                    .ToHashSet();

            var block2Queue = usedPositions
                .ToDictionary(k => new Block(),
                              v => new Dictionary<(int x, int y), (float curr, float need)>() { { v, (cellularMap[v], cellularMap[v]) } });

            while (block2Queue.Keys.Sum(x => x.Count) != cellularMap.Count)
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

                    foreach (var next in Hexagon.GetNeighbors(pos).Where(n => cellularMap.ContainsKey(n)))
                    {
                        if (usedPositions.Contains(next))
                        {
                            continue;
                        }

                        pos2Fill.Add(next, (0f, cellularMap[next]));
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
