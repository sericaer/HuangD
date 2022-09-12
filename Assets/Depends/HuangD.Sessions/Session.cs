using HuangD.Entities;
using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions
{
    public partial class Session : ISession
    {
        public string seed { get; private set; }
        public IMap map { get; set; }

        public IEnumerable<IProvince> provinces { get; private set; }
        public IEnumerable<ICountry> countries { get; private set; }
        public IEnumerable<IPerson> persons { get; private set; }

        public ICountry playerCountry { get; set; }

        public Dictionary<ICountry, List<IProvince>> country2Provinces { get; private set; }
        public Dictionary<IProvince, Block> province2Block { get; private set; }

        private HashSet<Peson2OfficeItem> person2Office { get; set; }

        private class Peson2OfficeItem
        {
            public IPerson person;
            public IOffice office;

            public Peson2OfficeItem(IPerson person, IOffice office)
            {
                this.person = person;
                this.office = office;
            }
        }

        public Session()
        {
            Country.funcGetProvinces = (country) => country2Provinces[country];
            Province.funcGetBlock = (province) => province2Block[province];
            Office.funcGetPerson = (office) => person2Office.SingleOrDefault(x => x.office == office)?.person;
            Person.funcGetOffice = (person) => person2Office.SingleOrDefault(x => x.person == person)?.office;
        }
    }
}