using HuangD.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods.Interfaces
{
    public interface IProvinceDef
    {
        public IEnumerable<string> names { get; }
        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; }
    }

    public interface IBufferDef
    {
        IEnumerable<IEffectDef> effects { get; }
    }
}