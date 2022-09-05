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

        public Dictionary<ICountry, List<IProvince>> country2Provinces { get; private set; }
        public Dictionary<IProvince, Block> province2Block { get; private set; }

        public ICountry playerCountry { get; set; }

        public Session()
        {
            Country.funcGetProvinces = (country) => country2Provinces[country];
            Province.funcGetBlock = (province) => province2Block[province];
        }
    }
}