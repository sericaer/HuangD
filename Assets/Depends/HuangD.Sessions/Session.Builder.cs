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

                processInfo.Invoke("创建省份");
                var provinces = Province.Builder.Build(map, seed, defs.provinceNameDef);

                processInfo.Invoke("创建国家");
                var countries = Country.Builder.Build(provinces, seed, defs.countryNameDef);

                processInfo.Invoke("创建人物");
                var persons = Person.Builder.Build(countries.SelectMany(x => x.officeGroup.offices).Count(), seed, defs.personNameDef);

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
    }
}