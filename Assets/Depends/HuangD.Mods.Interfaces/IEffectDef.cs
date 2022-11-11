using HuangD.Interfaces;

namespace HuangD.Mods.Interfaces
{
    public interface IEffectDef
    {
        double factor { get; }
        IEffect.Target target { get; }
    }
}