using Hjson;
using System.IO;

namespace HuangD.Mods
{
    internal class ModFileSystem
    {
        public string personNames { get; }
        public string countryNames { get; }
        public string provinceNames { get; }

        public string popLiveliHood { get; }
        public string popTaxLevels { get; }

        public string modPath;

        private string personNameFile => Path.Combine(modPath, "Defines", "Person", "NameDef.hjson");
        private string countryNameFile => Path.Combine(modPath, "Defines", "Country", "NameDef.hjson");
        private string provinceNameFile => Path.Combine(modPath, "Defines", "Province", "NameDef.hjson");
        private string popLiveliHoodFile => Path.Combine(modPath, "Defines", "Pop", "LiveliHood.hjson");
        private string popTaxLevelsFile => Path.Combine(modPath, "Defines", "Pop", "popTaxLevels.hjson");
        public ModFileSystem(string modPath)
        {
            this.modPath = modPath;

            personNames = HjsonValue.Load(personNameFile).ToString();
            countryNames = HjsonValue.Load(countryNameFile).ToString();
            provinceNames = HjsonValue.Load(provinceNameFile).ToString();
            popLiveliHood = HjsonValue.Load(popLiveliHoodFile).ToString();
            popTaxLevels = HjsonValue.Load(popTaxLevelsFile).ToString();
        }
    }
}