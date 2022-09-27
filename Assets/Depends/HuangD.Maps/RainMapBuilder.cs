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
        private static Dictionary<(int x, int y), TerrainType> terrains;

        private static float totalRain = 30f;
        private static float maxRain = 1f;

        internal static Dictionary<(int x, int y), float> Build(Dictionary<(int x, int y), TerrainType> terrains, GRandom random)
        {
            RainMapBuilder.random = random;
            RainMapBuilder.terrains = terrains;

            var waterPositions = terrains.Where(p => p.Value == TerrainType.Water)
                .Select(x => x.Key)
                .ToHashSet();

            var landPostions = terrains.Keys.Except(waterPositions)
                .ToHashSet();

            var origins = waterPositions.Where(x => Hexagon.GetNeighbors(x).Any(n => landPostions.Contains(n)))
                .ToHashSet();

            var rainMap = waterPositions.ToDictionary(k => k, v => new RainItem(totalRain, v, v));

            var queue = new UniqueQueue<(int x, int y)>(origins);
            while (queue.Count != 0)
            {
                var currPos = queue.Dequeue();
                var currValue = rainMap[currPos];

                foreach (var next in Hexagon.GetNeighbors(currPos).Where(n => landPostions.Contains(n)))
                {
                    var nextTotal = currValue.nextTotal;
                    if (!rainMap.ContainsKey(next) || nextTotal > rainMap[currPos].total)
                    {
                        rainMap[next] = new RainItem(nextTotal, next, currPos);
                        queue.Enqueue(next);
                    }
                }
            }

            var overloads = rainMap.Where(x => x.Value.curr > maxRain).Select(x=>x.Value).ToArray();
            foreach(var overload in overloads)
            {
                var overloadValue = overload.curr - maxRain;
                overload.curr = maxRain;

                var preItem = rainMap[overload.prePos];
                preItem.curr += overloadValue;

                if(preItem.curr > maxRain)
                {
                    OverloadSpread(preItem.currPos, rainMap);
                }
            }

            var rslt = rainMap.ToDictionary(k => k.Key, v => v.Value.curr);

            Debug.Log($"min rain {rslt.Min(x => x.Value)}, max rain {rslt.Max(x => x.Value)}");

            return rslt;
        }

        private static void OverloadSpread((int x, int y) overloadPos, Dictionary<(int x, int y), RainItem> rainMap)
        {

            var overloadValue = rainMap[overloadPos].curr - maxRain;
            rainMap[overloadPos].curr = maxRain;

            var queue = new UniqueQueue<(int x, int y)>();
            queue.Enqueue(overloadPos);

            while (queue.Count != 0 && overloadValue > 0)
            {
                var currPos = queue.Dequeue();

                var neighors = Hexagon.GetNeighbors(currPos).Where(n=>terrains.ContainsKey(n));
                foreach(var next in neighors)
                {
                    var currTerrainType = terrains[currPos];
                    var nextTerrainType = terrains[next];

                    if (nextTerrainType - currTerrainType > 0 )
                    {
                        continue;
                    }
                    if(rainMap[next].curr >= maxRain)
                    {
                        continue;
                    }

                    var nextValue = rainMap[next].curr + overloadValue;
                    if(nextValue > 1)
                    {
                        overloadValue = nextValue - maxRain;
                        rainMap[next].curr = maxRain;
                    }
                    else
                    {
                        rainMap[next].curr = nextValue;
                        overloadValue = -0;
                        break;
                    }

                    queue.Enqueue(next);
                }
            }

        }

        class RainItem
        {
            public float total;
            public float curr;
            public readonly (int x, int y) currPos;
            public readonly (int x, int y) prePos;

            public RainItem(float total, (int x, int y) curr, (int x, int y) prev)
            {
                this.total = total;
                this.curr = total / totalRain / 10 * 9;

                this.currPos = curr;
                this.prePos = prev;

                var currTerrain = terrains[curr];
                var prevTerrain = terrains[prev];

                if (currTerrain == TerrainType.Hill)
                {
                    if (prevTerrain == TerrainType.Water)
                    {
                        this.curr = this.curr * 3f;
                    }
                    if (prevTerrain == TerrainType.Plain)
                    {
                        this.curr = this.curr * 3f;
                    }
                }
                if (currTerrain == TerrainType.Mount)
                {
                    if (prevTerrain == TerrainType.Water)
                    {
                        this.curr = this.curr * 5f;
                    }
                    if (prevTerrain == TerrainType.Plain)
                    {
                        this.curr = this.curr * 5f;
                    }
                    if (prevTerrain == TerrainType.Hill)
                    {
                        this.curr = this.curr * 2f;
                    }
                }
            }

            public float nextTotal => total - curr;
        }
    }



    class UniqueQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private HashSet<T> hashSet = new HashSet<T>();

        public UniqueQueue()
        {

        }

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
