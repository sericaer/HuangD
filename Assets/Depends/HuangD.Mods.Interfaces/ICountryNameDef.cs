using System.Collections.Generic;

namespace HuangD.Mods.Interfaces
{
    public interface ICountryNameDef
    {
        IEnumerable<string> names { get; }
    }
}