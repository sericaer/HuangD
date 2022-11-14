using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class BufferDef : IBufferDef
    {
        [JsonConverter(typeof(InterfaceConverter<IEffectDef, EffectDef>))]
        public IEnumerable<IEffectDef> effects { get; set; }
    }
}