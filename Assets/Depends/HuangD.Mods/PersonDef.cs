using HuangD.Mods.Inferfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    public class PersonDef : IPersonDef
    {
        public IEnumerable<string> familyNames { get; internal set; }

        public IEnumerable<string> givenNames { get; internal set; }
    }
}