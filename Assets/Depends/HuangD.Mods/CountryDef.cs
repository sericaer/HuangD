using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class CountryDef : ICountryDef
    {
        public IEnumerable<string> names { get; internal set; }
    }
}