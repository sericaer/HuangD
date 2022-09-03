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

    }
}
