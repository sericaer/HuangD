using HuangD.Mods.Inferfaces;

namespace HuangD.Mods
{
    public partial class Mod : IMod
    {
        public IDefs defs { get; internal set; }
    }
}