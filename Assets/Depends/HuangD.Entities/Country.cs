using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Country : ICountry
    {
        internal static Func<ICountry, IEnumerable<IProvince>> funcGetProvinces;

        public string name { get; set; }
        public (int r, int g, int b) color { get; }
        public IOfficeGroup officeGroup { get; }
        public IEnumerable<IProvince> provinces { get; }
        public IPerson leader => officeGroup.leaderOffice.person;

        public Country(string name, IEnumerable<IProvince> provinces, (int r, int g, int b) color)
        {
            this.name = name;
            this.color = color;
            this.provinces = provinces.ToList();

            this.officeGroup = new OfficeGroup(this);
        }
    }
}
