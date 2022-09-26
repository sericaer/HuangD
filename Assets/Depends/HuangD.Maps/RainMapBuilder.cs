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
    public class RainMapBuilder
    {
        private static GRandom random;

        internal static Dictionary<(int x, int y), float> Build(Dictionary<(int x, int y), TerrainType> terrains, Dictionary<Block, TerrainType> block2Terrain, GRandom random)
        {
            RainMapBuilder.random = random;

            var totalPos = block2Terrain.Where(p => p.Value != TerrainType.Water)
                .SelectMany(x => x.Key.elements)
                .ToHashSet();

            var origins = FindCoastlinePositions(block2Terrain);


            var wetnessMap = origins.ToDictionary(k => k, v => new WetItem(30, TerrainType.Water, terrains[v]));
            var queue = new UniqueQueue<(int x, int y)>(origins);
            while (queue.Count != 0)
            {
                var currPos = queue.Dequeue();
                var currValue = wetnessMap[currPos];

                foreach (var next in Hexagon.GetNeighbors(currPos).Where(n => totalPos.Contains(n)))
                {
                    var nextTotal = currValue.nextTotal;
                    if (!wetnessMap.ContainsKey(next) || nextTotal > wetnessMap[currPos].total)
                    {
                        wetnessMap[next] = new WetItem(nextTotal, terrains[next], terrains[currPos]);
                        queue.Enqueue(next);
                    }
                }
            }

            Debug.Log($"min wet {wetnessMap.Min(x => x.Value.curr)}");

            return wetnessMap.ToDictionary(k => k.Key, v => v.Value.curr);
        }

        private static HashSet<(int x, int y)> FindCoastlinePositions(Dictionary<Block, TerrainType> block2Terrain)
        {
            var waterBlocks = block2Terrain.Keys.Where(k => block2Terrain[k] == TerrainType.Water);

            var origins = new HashSet<(int x, int y)>();
            foreach (var block in waterBlocks)
            {
                foreach (var neighbor in block.neighors.Where(b => block2Terrain[b] != TerrainType.Water))
                {
                    foreach (var edge in neighbor.edges.Where(x => block.edges.Any(y => Hexagon.isNeighbor(x, y))))
                    {
                        origins.Add(edge);
                    }
                }
            }

            return origins;
        }


    }

    class WetItem
    {
        public float total;
        public float curr;

        public WetItem(float total, TerrainType prev, TerrainType curr)
        {
            this.total = total;
            this.curr = total / 30;

            if (curr == TerrainType.Hill)
            {
                if (prev == TerrainType.Water)
                {
                    this.curr = this.curr * 2f;
                }
                if (prev == TerrainType.Plain)
                {
                    this.curr = this.curr * 2f;
                }
            }
            if (curr == TerrainType.Mount)
            {
                if (prev == TerrainType.Water)
                {
                    this.curr = this.curr * 4f;
                }
                if (prev == TerrainType.Plain)
                {
                    this.curr = this.curr * 4f;
                }
                if (prev == TerrainType.Hill)
                {
                    this.curr = this.curr * 1.5f;
                }
            }
        }

        public float nextTotal => total - curr;
    }

    class UniqueQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private HashSet<T> hashSet = new HashSet<T>();

        public UniqueQueue(HashSet<T> hasSet)
        {
            foreach (var item in hasSet)
            {
                Enqueue(item);
            }
        }

        public virtual void Enqueue(T item)
        {
            if (hashSet.Add(item)) { queue.Enqueue(item); }
        }
        public int Count { get { return queue.Count; } }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            hashSet.Remove(item);

            return item;
        }
    }
}
