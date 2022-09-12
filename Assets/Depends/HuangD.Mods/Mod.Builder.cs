using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace HuangD.Mods
{
    public partial class Mod
    {
        public static class Builder
        {
            public static IMod Build(string rootPath)
            {
                var modPath = Path.Combine(rootPath, "native");

                var native = new Mod(modPath);

                return native;
            }
        }
    }
}