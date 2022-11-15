using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Mods
{
    internal class BufferDef : IBufferDef
    {
        [JsonConverter(typeof(ConcreteTypeConverter<IEnumerable<EffectDef>>))]
        public IEnumerable<IEffectDef> effects { get; set; }

    }
}