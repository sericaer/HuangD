using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using Hjson;

namespace HuangD.Mods
{
    public partial class Mod : IMod
    {
        public IDefs defs { get; internal set; }

        private ModFileSystem fileSystem;

        public Mod(string modPath)
        {
            fileSystem = new ModFileSystem(modPath);

            defs = new Defs(fileSystem);


        }

        private Mod()
        {

        }
    }
}