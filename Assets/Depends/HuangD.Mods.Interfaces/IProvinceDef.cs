using HuangD.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods.Interfaces
{
    public interface IProvinceDef
    {
        public IEnumerable<string> names { get; }
    }

    public interface IBufferDef
    {
        IEnumerable<IEffectDef> effects { get; }
    }

    public interface IPopDef
    {
        double maxLiveliHood { get; }
        double minLiveliHood { get; }

        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }
    }
}