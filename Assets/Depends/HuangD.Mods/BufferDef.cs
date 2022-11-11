using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Mods
{
    internal class BufferDef : IBufferDef
    {
        public IEnumerable<IEffectDef> effects { get; set; }
    }
}