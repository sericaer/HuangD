using HuangD.Mods.Interfaces;
using Newtonsoft.Json;

namespace HuangD.Mods
{
    internal class Defs : IDefs
    {
        public IPersonNameDef personNameDef { get; internal set; }
        public ICountryNameDef countryNameDef { get; internal set; }

        public IProvinceNameDef provinceNameDef { get; internal set; }

        public Defs(ModFileSystem fileSystem)
        {
            personNameDef = JsonConvert.DeserializeObject<PersonNameDef>(fileSystem.personNames);
            countryNameDef = JsonConvert.DeserializeObject<CountryNameDef>(fileSystem.countryNames);
            provinceNameDef = JsonConvert.DeserializeObject<ProvinceNameDef>(fileSystem.provinceNames);
        }

        internal Defs()
        {

        }
    }
}