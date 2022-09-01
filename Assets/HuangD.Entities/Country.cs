using HuangD.Interfaces;
using System;
using System.Collections.Generic;

namespace HuangD.Entities
{
    public partial class Country : ICountry
    {
        internal static Func<ICountry, IEnumerable<IProvince>> funcGetProvinces;

        public string name { get; set; }

        public IEnumerable<IProvince> provinces => funcGetProvinces(this);

        public (float r, float g, float b) color { get; }

        public Country(string name, (float, float, float) color)
        {
            this.name = name;
            this.color = color;
        }
    }
}
