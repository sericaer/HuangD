using Hjson;
using System.IO;

namespace HuangD.Mods
{
    internal class ModFileSystem
    {
        public string personNames { get; }
        public string countryNames { get; }
        public string provinceNames { get; }

        public string modPath;

        private string personNameFile => Path.Combine(modPath, "Defines", "Person", "NameDef.hjson");
        private string countryNameFile => Path.Combine(modPath, "Defines", "Country", "NameDef.hjson");
        private string provinceNameFile => Path.Combine(modPath, "Defines", "Province", "NameDef.hjson");
        public ModFileSystem(string modPath)
        {
            this.modPath = modPath;

            personNames = HjsonValue.Load(personNameFile).ToString();
            countryNames = HjsonValue.Load(countryNameFile).ToString();
            provinceNames = HjsonValue.Load(provinceNameFile).ToString();
        }
    }
}