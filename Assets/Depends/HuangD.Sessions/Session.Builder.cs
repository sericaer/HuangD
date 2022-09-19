using HuangD.Entities;
using HuangD.Interfaces;
using HuangD.Maps;
using HuangD.Mods.Interfaces;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions
{


    public partial class Session
    {
        public static class Builder
        {
            public static ISession Build(MapInit mapInit, string seed, IDefs defs, Action<string> processInfo)
            {
                var map = Map.Builder.Build(mapInit, seed, processInfo);

                var noWaterBlocks = map.blocks.Where(x => x.Value != TerrainType.Water).Select(x => x.Key);

                processInfo.Invoke("创建省份");
                var provinces = Province.Builder.Build(noWaterBlocks.Count(), seed, defs.provinceNameDef);

                processInfo.Invoke("创建国家");
                var countries = Country.Builder.Build(provinces.Count() / 3, seed, defs.countryNameDef);

                processInfo.Invoke("创建人物");
                var persons = Person.Builder.Build(countries.SelectMany(x=>x.officeGroup.offices).Count(), seed, defs.personNameDef);

                var session = new Session();
                session.seed = seed;
                session.map = map;
                session.provinces = provinces;
                session.countries = countries;
                session.persons = persons;

                session.playerCountry = countries.First();

                processInfo.Invoke("关联数据");
                session.AssocateData(seed);

                return session;
            }
        }

        private void AssocateData(string seed)
        {
            var random = new GRandom(seed);

            SetProvince2Block(random);
            SetCountry2Provinces(random);
            SetPerson2Office(random);
        }

        private void SetPerson2Office(GRandom random)
        {
            person2Office = new HashSet<Peson2OfficeItem>();

            var person = persons.FirstOrDefault(x => x.office == null);
            while(person != null)
            {
                foreach (var country in countries)
                {
                    var emptyOffice = country.officeGroup.offices.FirstOrDefault(x => x.person == null);
                    if (emptyOffice == null)
                    {
                        continue;
                    }

                    person2Office.Add(new Peson2OfficeItem(person, emptyOffice));

                    person = persons.FirstOrDefault(x => x.office == null);
                }

                person = persons.FirstOrDefault(x => x.office == null);
            }
        }

        private void SetCountry2Provinces(GRandom random)
        {
            var originIndexs = Enumerable.Range(0, provinces.Count())
                .OrderBy(_ => random.getNum(0, int.MaxValue))
                .Take(countries.Count())
                .ToArray();

            country2Provinces = new Dictionary<ICountry, List<IProvince>>();
            for (int i = 0; i < countries.Count(); i++)
            {
                var list = new List<IProvince>();
                list.Add(provinces.ElementAt(originIndexs[i]));

                country2Provinces.Add(countries.ElementAt(i), list);
            }

            var originProvinces = new List<IProvince>(provinces.Except(countries.SelectMany(x=>x.provinces)));

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
            var vaildProvinceTerrains = new TerrainType[] { TerrainType.Hill, TerrainType.Mount, TerrainType.Plain };

            var vaildProvinceBlocks = map.blocks.Where(x => vaildProvinceTerrains.Contains(x.Value)).Select(x => x.Key);

            province2Block = Enumerable.Range(0, provinces.Count())
                .ToDictionary(i => provinces.ElementAt(i), j => vaildProvinceBlocks.ElementAt(j));
        }
    }
}