using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Interfaces
{
    public interface ISession
    {
        public string seed { get; }

        public IMap map { get; set; }
        public IEnumerable<ICountry> countries { get; }
        public IEnumerable<IProvince> provinces { get; }
        public IEnumerable<IPerson> persons { get; }

        public ICountry playerCountry { get; set; }

        public IDate date { get; set; }
    }
}


