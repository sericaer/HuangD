using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface ICountry
    {
        public string name { get; }
        public (int r, int g, int b) color { get; }

        public IEnumerable<IProvince> provinces { get; }

        public IPerson leader { get; }

        public IOfficeGroup officeGroup { get; }

        public ITreasury treasury { get; }

        public IMilitary military { get; }

        void OnDaysInc(int year, int month, int day);
    }
}


