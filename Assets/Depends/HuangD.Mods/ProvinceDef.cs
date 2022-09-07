using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class ProvinceDef : IProvinceDef
    {
        public IEnumerable<string> names { get; internal set; }
    }
}