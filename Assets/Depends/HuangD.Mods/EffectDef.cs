using HuangD.Interfaces;
using HuangD.Mods.Interfaces;

namespace HuangD.Mods
{
    internal class EffectDef : IEffectDef
    {
        public double factor { get; set; }

        public IEffect.Target target { set; get; }
    }
}