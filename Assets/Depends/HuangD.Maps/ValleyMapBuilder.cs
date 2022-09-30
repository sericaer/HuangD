using Math.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    internal class ValleyMapBuilder
    {
        class LinkItem
        {
            public (int x, int y) currPos;
            public (int x, int y)? nextPos;
            public List<(int x, int y)> prevPositions = new List<(int x, int y)>();
        }

        internal static IEnumerable<IEnumerable<(int x, int y)>> Build(Dictionary<(int x, int y), float> heightMap)
        {
            var topo = new Dictionary<(int x, int y), LinkItem>();

            foreach(var currPos in heightMap.Keys.Where(k=> heightMap[k] > 0))
            {
                LinkItem item;

                if(!topo.TryGetValue(currPos, out item))
                {
                    item = new LinkItem() { currPos = currPos };
                    topo.Add(currPos, item);
                }

                if (item.nextPos != null)
                {
                    throw new Exception();
                }

                var nextLowest = Hexagon.GetNeighbors(currPos)
                        .Where(x => heightMap.ContainsKey(x))
                        .OrderBy(x => heightMap[x])
                        .First();

                if(heightMap[nextLowest] < heightMap[currPos])
                {
                    item.nextPos = nextLowest;

                    LinkItem nextItem;

                    if (!topo.TryGetValue(nextLowest, out nextItem))
                    {
                        nextItem = new LinkItem() { currPos = nextLowest };
                        topo.Add(nextLowest, nextItem);
                    }

                    nextItem.prevPositions.Add(currPos);
                }
            }

            var origins = topo.Values.Where(x => x.prevPositions.Count != 1);

            var valleys = new List<List<(int x, int y)>>();
            foreach(var origin in origins)
            {
                var valley = new List<(int x, int y)>();
                valleys.Add(valley);

                var curr = origin;

                while(true)
                {
                    if (curr.nextPos == null)
                    {
                        break;
                    }

                    var next = topo[curr.nextPos.Value];
                    if (next.prevPositions.Count != 1)
                    {
                        break;
                    }

                    valley.Add(next.currPos);

                    curr = next;
                }
                
            }

            return valleys.Where(x=>x.Count > 1);
        }
    }
}