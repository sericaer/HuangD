using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HuangD.Maps
{
    public static class HeightMapBuilder
    {
        class Block
        {
            public int id;
            public (int x, int y)[] elements;
            public (int x, int y)[] edges;
            public int[] neighors;
        }

        internal static Dictionary<(int x, int y), float> Build(Dictionary<(int x, int y), BlockInfo> blockMap, Dictionary<(int x, int y), float> noiseMap, GRandom random)
        {
            Debug.Log("Build 0");

            var blocks = blockMap.GroupBy(x => x.Value.blockId)
                    .Select(x => new Block()
                    {
                        id = x.Key,
                        elements = x.Select(y => y.Key).ToArray(),
                        edges = x.Where(y=>y.Value.isEdge).Select(y => y.Key).ToArray(),
                        neighors = x.SelectMany(y=>y.Value.neighborBlockIds).Distinct().ToArray()
                    }).ToArray();

            Debug.Log("Build 1");


            Dictionary<int, int> dictSouthOrder = GenerateEdgeOrder(blocks, blocks.Where(x => x.edges.Any(e => e.x == 0)).ToArray());
            Dictionary<int, int> dictEastOrder = GenerateEdgeOrder(blocks, blocks.Where(x => x.edges.Any(e => e.y == noiseMap.Keys.Max(k => k.y))).ToArray());

            Dictionary<int, int> dictOrderFactor = blocks.Select(x => x.id).ToDictionary(k => k, k => System.Math.Min(dictSouthOrder[k], dictEastOrder[k]));

            var hegihtFactors = new Dictionary<(int x, int y), float>();

            foreach(var block in blocks)
            {
                var randomFactor = random.isTrue(50) ? 1f : -1f;

                var orderFactor = dictOrderFactor[block.id];
                if(orderFactor <= 1)
                {
                    orderFactor = 0;
                }

                foreach (var pos in block.elements)
                {
                    if(orderFactor == 0)
                    {
                        hegihtFactors.Add(pos, orderFactor);
                    }
                    else
                    {
                        hegihtFactors.Add(pos, orderFactor + noiseMap[pos] * randomFactor);
                    }

                }
            }

            var rslt = new Dictionary<(int x, int y), float>();

            var hegihtOrderFactors = hegihtFactors.OrderBy(x => x.Value).ToArray();

            for(int i=0; i< hegihtOrderFactors.Length; i++)
            {
                var key = hegihtOrderFactors[i].Key;
                var height = i * 1f / hegihtOrderFactors.Length;

                rslt.Add(key, hegihtFactors[key] < 0.00000000001f ? 0 : height);
            }

            Debug.Log("Build 2");

            return rslt;
        }

        private static Dictionary<int, int> GenerateEdgeOrder(IEnumerable<Block> blocks, Block[] originBlocks)
        {
            var allBlocks = blocks.ToDictionary(v => v.id, v => v);

            var queue = new Queue<Block>(originBlocks);

            var rslt = originBlocks.Select(x => x.id).ToDictionary(k => k, _ => 0);

            while (queue.Count != 0)
            {
                var curr = queue.Dequeue();

                foreach (var nextId in curr.neighors)
                {
                    if(!rslt.ContainsKey(nextId) ||  rslt[curr.id] + 1 < rslt[nextId])
                    {
                        rslt[nextId] = rslt[curr.id] + 1;
                        queue.Enqueue(allBlocks[nextId]);
                    }
                }
            }

            return rslt;
        }
    }
}
