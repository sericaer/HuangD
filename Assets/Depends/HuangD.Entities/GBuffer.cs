using HuangD.Effects;
using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    internal class GBuffer : IBuffer
    {
        public string key { get; }

        public IEnumerable<IEffect> effects { get; }

        public IBufferDef def { get; }

        public GBuffer(string from, IBufferDef def)
        {
            this.key = from;
            this.def = def;

            effects = def.effects.Select(x => new Effect(x, this)).ToArray();
        }
    }
}