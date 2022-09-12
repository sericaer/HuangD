using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    public class PersonNameDef : IPersonNameDef
    {
        public IEnumerable<string> familys { get;  set; }

        public IEnumerable<string> givens { get;  set; }
    }
}