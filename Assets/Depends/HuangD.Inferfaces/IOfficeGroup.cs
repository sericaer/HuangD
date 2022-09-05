using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IOfficeGroup
    {
        public IOffice leaderOffice { get; }
        public IEnumerable<IOffice> offices { get; }
    }
}