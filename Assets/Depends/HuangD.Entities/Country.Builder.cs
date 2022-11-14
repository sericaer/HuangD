using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Entities
{
    public partial class Country
    {
        private class ProvinceGroup
        {
            public List<IProvince> provinces = new List<IProvince>();
            public HashSet<ProvinceGroup> neighbors = new HashSet<ProvinceGroup>();
        }

        public static class Builder
        {
            private static ICountryNameDef def;
            private static HashSet<(int r, int g, int b)> colors;
            private static GRandom random;

            private const int maxProvinceCount = 6;
            private const int minProvinceCount = 2;
            private const int maxPopulationCount = 20 * 10000;
            private const int minPopulationCount = 5 * 10000;

            public static IEnumerable<ICountry> Build(IEnumerable<IProvince> provinces, string seed, ICountryNameDef def)
            {
                Builder.def = def;
                Builder.random = new GRandom(seed);
                Builder.colors = new HashSet<(int r, int g, int b)>();

                List<ProvinceGroup> groups = GenerateProvinceGroups(provinces);

                var randomNames = Builder.def.names.OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

                Debug.Log($"randomNames{randomNames.Length}, groups{groups.Count}");

                var list = new List<ICountry>();
                for (int i = 0; i < groups.Count; i++)
                {
                    list.Add(new Country(randomNames[i], groups[i].provinces, GenerateRandomColor()));
                }

                return list;
            }

            private static (int r, int g, int b) GenerateRandomColor()
            {

                while(true)
                {
                    var color = (random.getNum(0, 255), random.getNum(0, 255), random.getNum(0, 255));
                    if(!colors.Contains(color))
                    {
                        colors.Add(color);
                        return color;
                    }
                }
            }

            private static List<ProvinceGroup> GenerateProvinceGroups(IEnumerable<IProvince> provinces)
            {
                var vaildProvinces = provinces.ToHashSet();

                var rslt = new List<ProvinceGroup>();
                var prov2Country = new Dictionary<IProvince, ProvinceGroup>();

NewGroup:
                while (vaildProvinces.Count != 0)
                {
                    var start = vaildProvinces.First();
                    vaildProvinces.Remove(start);

                    var group = new ProvinceGroup();
                    rslt.Add(group);

                    group.provinces.Add(start);

                    var queue = new Queue<IProvince>();
                    queue.Enqueue(start);

                    prov2Country.Add(start, group);

                    while (queue.Count != 0)
                    {
                        var curr = queue.Dequeue();

                        foreach (var next in curr.neighbors)
                        {
                            if(!vaildProvinces.Contains(next))
                            {
                                if(!group.provinces.Contains(next))
                                {
                                    var other = prov2Country[next];

                                    other.neighbors.Add(group);
                                    group.neighbors.Add(other);
                                }
                                continue;
                            }

                            vaildProvinces.Remove(next);

                            queue.Enqueue(next);
                            group.provinces.Add(next);

                            prov2Country.Add(next, group);

                            if (group.provinces.Count > maxProvinceCount || (group.provinces.Count > minProvinceCount && group.provinces.Sum(p=>p.pop.count) > maxPopulationCount))
                            {
                                goto NewGroup;
                            }
                        }
                    }
                }

                var smallGroups = new Queue<ProvinceGroup>(rslt.Where(x => x.neighbors.Count > 0)
                    .Where(x=> x.provinces.Count < minProvinceCount || x.provinces.Sum(p => p.pop.count) < minPopulationCount));

                rslt.RemoveAll(x => smallGroups.Contains(x));

                while (smallGroups.Count != 0)
                {
                    var curr = smallGroups.Dequeue();

                    var mergeTo = curr.neighbors
                        .Where(x=> rslt.Contains(x))
                        .OrderBy(x => x.provinces.Sum(p => p.pop.count)).FirstOrDefault();
                    if(mergeTo == null)
                    {
                        smallGroups.Enqueue(curr);
                        continue;
                    }

                    mergeTo.provinces.AddRange(curr.provinces);

                    foreach (var neighbor in curr.neighbors.Where(x => x != mergeTo))
                    {
                        mergeTo.neighbors.Add(neighbor);

                        neighbor.neighbors.Remove(curr);
                        neighbor.neighbors.Add(mergeTo);
                    }
                }

                foreach (var group in rslt)
                {
                    group.neighbors.IntersectWith(rslt);
                }

                return rslt;
            }
        }


    }
}
