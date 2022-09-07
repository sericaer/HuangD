using System.Collections.Generic;

namespace HuangD.Mods.Interfaces
{
    public interface ICountryDef
    {
        IEnumerable<string> names { get; }
    }
}