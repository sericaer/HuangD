using Math.TileMap;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public static class Utilty
    {
        public static Dictionary<(int x, int y), int> GenerateEdges(IEnumerable<IEnumerable<(int x, int y)>> edgeGroups)
        {
            var dict = new Dictionary<(int x, int y), IEnumerable<(int x, int y)>>();
            foreach (var group in edgeGroups)
            {
                foreach (var edge in group.Select(e => Hexagon.ScaleOffset(e, 2)))
                {
                    if (dict.ContainsKey(edge))
                    {
                        continue;
                    }
                    dict.Add(edge, group);
                }
            }

            var rlst = new Dictionary<(int x, int y), int>();
            var edgeCenters = edgeGroups.SelectMany(x => x)
                .Select(x => Hexagon.ScaleOffset(x, 2))
                .ToHashSet();

            var mirrorDrects = new (int d1, int d2)[]
            {
            (0, 3),
            (2, 5),
            (1, 4),
            };

            foreach (var egde in edgeCenters.SelectMany(e => Hexagon.GetNeighbors(e)).Distinct())
            {
                for (int i = 0; i < mirrorDrects.Length; i++)
                {
                    var mirrorDiect = mirrorDrects[i];

                    var neighbor1 = Hexagon.GetNeighbor(egde, mirrorDiect.d1);
                    var neighbor2 = Hexagon.GetNeighbor(egde, mirrorDiect.d2);

                    if (!dict.ContainsKey(neighbor1) || !dict.ContainsKey(neighbor2))
                    {
                        continue;
                    }

                    if (dict[neighbor1] != dict[neighbor2])
                    {
                        rlst.Add(egde, i);
                        break;
                    }
                }
            }

            return rlst;
        }

        public static (int x, int y) GetCenterPos(IEnumerable<(int x, int y)> elements)
        {
            IEnumerable<(int x, int y)> rings;

            int max = 0;
            int index = -1;
            for (int i = 0; i < elements.Count(); i++)
            {
                var elem = elements.ElementAt(i);
                int distance = 1;
                do
                {
                    rings = Hexagon.GetRing(elem, distance);
                    distance++;
                }
                while (rings.All(x => elements.Contains(x)));
                if (distance > max)
                {
                    index = i;
                    max = distance;
                }
            }

            return elements.ElementAt(index);
        }

        public static IEnumerable<(int x, int y)> FindPath((int x, int y) startPos, System.Func<(int x, int y), bool> endCondition, IEnumerable<(int x, int y)> baseMap, Dictionary<(int x, int y), int> costMap = null)
        {
            var visitMap = costMap == null ? GenrateVisitMap(startPos, endCondition, baseMap.ToHashSet()) : GenrateVisitMapWithCost(startPos, endCondition, baseMap.ToHashSet(), costMap);

            var path = new List<(int x, int y)>();
            try
            {
                var pos = visitMap.Keys.First(x => endCondition(x));
                path.Add(pos);
            }
            catch
            {
                return null;
            }

            var currPos = path.First();

            while (currPos != startPos)
            {
                var nextPos = visitMap[currPos];
                path.Add(nextPos);

                currPos = nextPos;
            }

            path.Reverse();
            return path;
        }

        private static Dictionary<(int x, int y), (int x, int y)> GenrateVisitMapWithCost((int x, int y) startPos, System.Func<(int x, int y), bool> endCondition, HashSet<(int x, int y)> baseMap, Dictionary<(int x, int y), int> costMap)
        {
            var queue = new PriorityQueue<(int x, int y)>();
            queue.Enqueue(startPos, 0);

            var costRecord = new Dictionary<(int x, int y), int>();
            costRecord[startPos] = 0;

            var dict = new Dictionary<(int x, int y), (int x, int y)>();
            dict.Add(startPos, (-1, -1));

            while (queue.Count != 0)
            {
                var currPos = queue.Dequeue();

                if (endCondition(currPos))
                {
                    break;
                }

                foreach (var neighor in Hexagon.GetNeighbors(currPos).Where(n => baseMap.Contains(n)))
                {
                    var newCost = costRecord[currPos] + costMap[neighor];

                    if (dict.ContainsKey(neighor) && costRecord[neighor] <= newCost)
                    {
                        continue;
                    }

                    costRecord[neighor] = newCost;
                    queue.Enqueue(neighor, newCost);

                    dict.Add(neighor, currPos);
                }
            }

            return dict;
        }

        private  static Dictionary<(int x, int y), (int x, int y)> GenrateVisitMap((int x, int y) startPos, System.Func<(int x, int y), bool> endCondition, HashSet<(int x, int y)> baseMap)
        {
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(startPos);

            var dict = new Dictionary<(int x, int y), (int x, int y)>();
            dict.Add(startPos, (-1, -1));

            while (queue.Count != 0)
            {
                var currPos = queue.Dequeue();

                if (endCondition(currPos))
                {
                    break;
                }

                foreach (var neighor in Hexagon.GetNeighbors(currPos).Where(n => baseMap.Contains(n)))
                {
                    if (dict.ContainsKey(neighor))
                    {
                        continue;
                    }

                    queue.Enqueue(neighor);
                    dict.Add(neighor, currPos);


                }
            }

            return dict;
        }

    }
}
