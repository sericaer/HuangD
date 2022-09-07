using HuangD.Mods.Interfaces;

namespace HuangD.Mods
{
    public partial class Mod : IMod
    {
        public IDefs defs { get; internal set; }
    }
}