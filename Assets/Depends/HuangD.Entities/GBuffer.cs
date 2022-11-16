using HuangD.Effects;
using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    internal class GBuffer : IBuffer
    {
        public string title => def.title;
        public IEnumerable<IEffect> effects { get; }

        public IBufferDef def { get; }

        public GBuffer(IBufferDef def)
        {
            this.def = def;

            effects = def.effects.Select(x => new Effect(x, this)).ToArray();
        }
    }
}