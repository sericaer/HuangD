using HuangD.Entities;
using HuangD.Interfaces;
using HuangD.Maps;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions
{
    public partial class Session
    {
        static TerrainType[] vaildProvinceTerrains { get; } = new TerrainType[] { TerrainType.Hill, TerrainType.Mount, TerrainType.Plain };

        public static class Builder
        {
            public static ISession Build(int mapSize, string seed)
            {
                var map = Map.Builder.Build(mapSize, seed);

                var noWaterBlocks = map.blocks.Where(x => x.Value != TerrainType.Water).Select(x => x.Key);

                var provinces = Province.Builder.Build(noWaterBlocks.Count(), seed);
                var countries = Country.Builder.Build(provinces.Count() / 3, seed);

                var session = new Session();
                session.seed = seed;
                session.map = map;
                session.provinces = provinces;
                session.countries = countries;

                session.AssocateData(seed);

                return session;
            }

            private static IDictionary<ICountry, List<IProvince>> BuildCountry2Provinces(IEnumerable<ICountry> countries, IEnumerable<IProvince> provinces)
            {
                throw new System.NotImplementedException();
            }
        }

        private void AssocateData(string seed)
        {
            var random = new GRandom(seed);

            SetProvince2Block(random);
            SetCountry2Provinces(random);
        }

        private void SetCountry2Provinces(GRandom random)
        {
            var originIndexs = Enumerable.Range(0, provinces.Count())
                .OrderBy(_ => random.getNum(0, int.MaxValue))
                .Take(countries.Count())
                .ToArray();

            var originProvinces = new List<IProvince>(provinces);

            country2Provinces = new Dictionary<ICountry, List<IProvince>>();
            for (int i = 0; i < countries.Count(); i++)
            {
                var list = new List<IProvince>();
                list.Add(originProvinces.ElementAt(originIndexs[i]));

                country2Provinces.Add(countries.ElementAt(i), list);
            }

            while (originProvinces.Count() != 0)
            {
                foreach(var provinceList in country2Provinces.Values)
                {
                    var curr = originProvinces.FirstOrDefault(x =>
                    {
                        return provinceList.Any(added => added.block.isNeighbor(x.block));
                    });

                    if (curr == null)
                    {
                        continue;
                    }

                    provinceList.Add(curr);
                    originProvinces.Remove(curr);
                }    
            }
        }

        private void SetProvince2Block(GRandom random)
        {
            var vaildProvinceBlocks = map.blocks.Where(x => vaildProvinceTerrains.Contains(x.Value)).Select(x => x.Key);

            province2Block = Enumerable.Range(0, provinces.Count())
                .ToDictionary(i => provinces.ElementAt(i), j => vaildProvinceBlocks.ElementAt(j));
        }
    }
}