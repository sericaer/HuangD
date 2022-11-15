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
        public LiveliHood liveliHood { get; }

        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }

        public class LiveliHood
        {
            public double max { get; set; }
            public double min { get; set; }

            public Dictionary<string, Level> levels { get; set; }

            public class Level
            {
                public (int min, int max) range { get; set; }
                public IBufferDef bufferDef { get; set; }
            }
        }
    }
}