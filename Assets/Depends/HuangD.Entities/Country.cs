using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Country : ICountry
    {
        public string name { get; set; }
        public (int r, int g, int b) color { get; }
        public IOfficeGroup officeGroup { get; }
        public IEnumerable<IProvince> provinces { get; }
        public IPerson leader => officeGroup.leaderOffice.person;

        public ITreasury treasury { get; }

        public Country(string name, IEnumerable<IProvince> provinces, (int r, int g, int b) color)
        {
            this.name = name;
            this.color = color;
            this.provinces = provinces.ToList();
            this.treasury = new Treasury(this);

            this.officeGroup = new OfficeGroup(this);
        }
    }
}
