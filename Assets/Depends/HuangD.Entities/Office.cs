using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public class Office : IOffice
    {
        public bool isLeader { get; }

        public string name { get; }

        internal static Func<IOffice, IPerson> funcGetPerson;

        public IPerson person => funcGetPerson(this);

        public ICountry country { get; }

        public Office(string name, bool isLeader, ICountry country)
        {
            this.name = name;
            this.isLeader = isLeader;
            this.country = country;
        }
    }

    public class OfficeGroup : IOfficeGroup
    {
        public IOffice leaderOffice => offices.Single(x => x.isLeader);

        public IEnumerable<IOffice> offices { get; }

        public OfficeGroup(ICountry country)
        {
            offices = new List<IOffice>()
            {
                new Office("OFFICE0", true, country)
            };
        }
    }
}
