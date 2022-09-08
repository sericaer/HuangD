using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class ProvinceNameDef : IProvinceNameDef
    {
        public IEnumerable<string> names { get;  set; }
    }
}