using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Mods
{
    internal partial class PopDef : IPopDef
    {
        public IPopDef.ILiveliHood liveliHood { get; set; }

        public Dictionary<ITreasury.CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }
        public Dictionary<IMilitary.CollectLevel, IBufferDef> ConscriptLevelBuffs { get; set; }

        public class Builder
        {
            public static PopDef Build(ModFileSystem fileSystem)
            {
                var taxLevelBuffs = JsonConvert.DeserializeObject<Dictionary<ITreasury.CollectLevel, BufferDef>>(fileSystem.popTaxLevels);
                foreach(var pair in taxLevelBuffs)
                {
                    if (pair.Value.title == null)
                    {
                        pair.Value.title = pair.Key.ToString();
                    }
                }

                var conscriptBuffs = JsonConvert.DeserializeObject<Dictionary<IMilitary.CollectLevel, BufferDef>>(fileSystem.conscriptLevels);
                foreach (var pair in conscriptBuffs)
                {
                    if (pair.Value.title == null)
                    {
                        pair.Value.title = pair.Key.ToString();
                    }
                }

                var def = new PopDef()
                {
                    liveliHood = JsonConvert.DeserializeObject<LiveliHood>(fileSystem.popLiveliHood),
                    popTaxLevelBuffs = taxLevelBuffs.ToDictionary(p => p.Key, p => (IBufferDef)p.Value),
                    ConscriptLevelBuffs = conscriptBuffs.ToDictionary(p=> p.Key, p => (IBufferDef)p.Value)
                };

                return def;
            }
        }
    }
}
