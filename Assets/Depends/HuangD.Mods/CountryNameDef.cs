using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class CountryNameDef : ICountryNameDef
    {
        public IEnumerable<string> names { get; set; }
    }
}