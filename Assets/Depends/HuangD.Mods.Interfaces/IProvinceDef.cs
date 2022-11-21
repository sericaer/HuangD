using HuangD.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuangD.Mods.Interfaces
{
    public interface IProvinceDef
    {
        public IEnumerable<string> names { get; }
    }

    public interface IBufferDef
    {
        public string title { get; }
        IEnumerable<IEffectDef> effects { get; }
    }

    public interface IPopDef
    {
        public ILiveliHood liveliHood { get; }

        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }
        public Dictionary<IMilitary.CollectLevel, IBufferDef> ConscriptLevelBuffs { get; set; }

        public interface ILiveliHood
        {
            public double max { get; set; }
            public double min { get; set; }

            public Dictionary<string, ILevel> levels { get; set; }

            public interface ILevel : IBufferDef
            {
                public Range range { get; set; }


                public class Range
                {
                    public int min { get; set; }
                    public int max { get; set; }
                }
            }
        }
    }
}