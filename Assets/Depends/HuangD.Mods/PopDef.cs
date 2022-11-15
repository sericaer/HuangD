using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Interfaces.ITreasury;

namespace HuangD.Mods
{
    internal partial class PopDef : IPopDef
    {
        public IPopDef.ILiveliHood liveliHood { get; set; }

        public Dictionary<CollectLevel, IBufferDef> popTaxLevelBuffs { get; set; }

        public class Builder
        {
            public static PopDef Build(ModFileSystem fileSystem)
            {
                var def = new PopDef()
                {
                    liveliHood = JsonConvert.DeserializeObject<LiveliHood>(fileSystem.popLiveliHood),
                    popTaxLevelBuffs = JsonConvert.DeserializeObject<Dictionary<CollectLevel, BufferDef>>(fileSystem.popTaxLevels)
                        .ToDictionary(p => p.Key, p => (IBufferDef)p.Value)
                };

                return def;
            }
        }
    }
}
