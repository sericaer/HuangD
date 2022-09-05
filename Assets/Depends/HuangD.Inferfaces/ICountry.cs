using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface ICountry
    {
        public (float r, float g, float b) color { get; }
        public string name { get; }
        public IEnumerable<IProvince> provinces { get; }

        public IPerson leader { get; }

        public IOfficeGroup officeGroup { get; }
    }
}


