using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal partial class ProvinceDef : IProvinceDef
    {
        public IEnumerable<string> names { get;  set; }

        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }
    }
}